import { Component, OnInit, ViewChild } from '@angular/core';
import { debounceTime, distinctUntilChanged, switchMap, catchError } from 'rxjs/operators';
import { CustomerService } from 'src/app/shared/services/customer/customer.service';
import { Observable, of, forkJoin, Subject } from 'rxjs';
import { NotificationService } from 'src/app/shared/services/notification/notification.service';
import { ProductService } from 'src/app/shared/services/product/product.service';
import { Product } from 'src/app/shared/models/product';
import { NgxSpinnerService } from 'ngx-spinner';
import { Customer } from 'src/app/shared/models/customerBase';
import { NgForm } from '@angular/forms';
import { MergedCustomer } from 'src/app/shared/models/mergedCustomer';
import { Order } from 'src/app/shared/models/order';
import { NgbDateStruct, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { MeasureUnit } from 'src/app/shared/models/measureUnit';
import { DomainService } from 'src/app/shared/services/domain/domain.service';
import { ProductQuote } from 'src/app/shared/models/productQuote';
import { PricingRequest } from 'src/app/shared/models/pricingRequest';
import { PricingService } from 'src/app/shared/services/pricing/pricing.service';
import { OrderItem } from 'src/app/shared/models/orderItem';
import { OrderService } from 'src/app/shared/services/order/order.service';
import { Router } from '@angular/router';
import { OrderDetails } from 'src/app/shared/models/orderDetails';

@Component({
  selector: 'app-new-order',
  templateUrl: './new-order.component.html',
  styleUrls: ['./new-order.component.scss']
})
export class NewOrderComponent implements OnInit {
  @ViewChild('f', { static: true }) form: NgForm;
  public title = 'Gerar Novo Pedido';
  public products: Product[] = [];
  public measures: MeasureUnit[] = [];
  public selectedCustomer: Customer;
  public order: Order = {} as Order;
  public minDate: NgbDateStruct;
  private modal: NgbModalRef;
  public currentQuote: ProductQuote = { orderItemId: null, orderItemStatus: null, quantity: 0, discount: 0, finalPrice: 0, measureUnit: null, price: 0, product: null, additionalInfo: '' };
  private qtdSubject: Subject<number> = new Subject();

  constructor(private customerService: CustomerService,
    private notificationService: NotificationService,
    private productService: ProductService,
    private domainService: DomainService,
    private modalService: NgbModal,
    private spinner: NgxSpinnerService,
    private pricingService: PricingService,
    private orderService: OrderService,
    private router: Router) {
    this.order.freightPrice = 0;
    this.order.orderItems = [];
    let today = new Date();
    this.minDate = {
      day: today.getUTCDate(),
      month: today.getMonth() + 1,
      year: today.getFullYear()
    };
    this.qtdSubject.pipe(debounceTime(500)).subscribe(r => this.calculatePrice());
  }

  ngOnInit(): void {
    this.spinner.show();
    let oMeasures = this.domainService.GetMeasureUnits();
    let oProducts = this.productService.GetAll();
    let orderNumber = history.state.data != null ? history.state.data.cloneFrom : null;
    let oOrder = of(null as OrderDetails);
    if (orderNumber != null) {
      oOrder = this.orderService.getByNumber(orderNumber);
    }
    forkJoin([oProducts, oMeasures, oOrder]).subscribe(resp => {
      this.products = resp[0] ?? [];
      this.measures = resp[1] ?? [];
      if (this.products.length > 0) {
        this.currentQuote.product = this.products[0];
        this.setMeasure();
      }
      if (this.measures.length > 0) {
        this.currentQuote.measureUnit = this.measures.filter((v, i) => v.measureUnitId == this.currentQuote.product.measureUnitId)[0];
      }
      this.spinner.hide();
      let r = resp[2];
      if (r == null) return;
      this.selectedCustomer = { ...r.customer };
      this.order.completeBy = new Date().toISOString();
      this.order.createdOn = r.createdOn;
      this.order.customerId = r.customer.customerId;
      this.order.freightPrice = r.freightPrice;
      this.order.lastUpdated = r.lastUpdated;
      this.order.orderId = 0;
      this.order.orderStatusId = r.orderStatusId;
      this.order.paymentStatusId = r.paymentStatusId;
      r.orderItems.forEach(i => {
        this.order.orderItems.push({
          additionalInfo: i.additionalInfo,
          discount: i.discount,
          measureUnitId: i.measureUnit.measureUnitId,
          orderItemStatusId: 1,
          productId: i.product.productId,
          product: i.product.name,
          originalPrice: i.originalPrice,
          priceAfterDiscount: i.priceAfterDiscount,
          quantity: i.quantity,
          measureUnit: i.measureUnit.description,
          measureUnitShort: i.measureUnit.shortName,
          orderId: 0,
          orderItemId: 0,
          createdOn: null,
          lastUpdated: null
        });
      });
      this.calculateOrderValue();
    }, (e) => {
      this.spinner.hide();
      this.notificationService.notifyHttpError(e);
    });
  }
  search = (text$: Observable<string>) => {
    return text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      switchMap((searchText) => {
        if (searchText.length < 2) return of(new Array<MergedCustomer>());
        return this.customerService.FilterMerged(searchText);
      }),
      catchError((e, c) => {
        this.notificationService.notifyHttpError(e);
        return e;
      })
    );
  }
  customerResultFormatter(value: any) {
    return value.name;
  }
  customerInputFormatter(value: any) {
    if (value.name) {
      return value.name.trim();
    }
    return value;
  }
  save() {
    if (!this.form.valid || this.order.orderItems.length == 0 || this.order.completeBy == null) return;
    this.spinner.show();
    this.order.customerId = this.selectedCustomer.customerId;
    this.order.paymentStatusId = 1;
    this.order.orderStatusId = 1;
    this.orderService.create(this.order).subscribe(r => {
      this.spinner.hide();
      this.notificationService.showSuccess('Sucesso', 'Pedido gerado com sucesso');
      this.router.navigate(['/order/details', r]);
    }, (e) => {
      this.spinner.hide();
      this.notificationService.notifyHttpError(e);
    });
  }
  setMeasure() {
    this.currentQuote.measureUnit = this.currentQuote.product == null ?
      this.measures[0] : this.measures.filter((v, i) => v.measureUnitId == this.currentQuote.product.measureUnitId)[0];
  }
  calculatePrice() {
    let request: PricingRequest = {
      resultPrecision: 2,
      productMeasureUnit: this.currentQuote.product.measureUnitId,
      productPrice: this.currentQuote.product.price,
      quantity: this.currentQuote.quantity,
      quantityMeasureUnit: this.currentQuote.measureUnit.measureUnitId
    };
    this.pricingService.calculateProductPrice(request).subscribe(r => {
      this.currentQuote.price = r;
      this.currentQuote.finalPrice = this.currentQuote.price - this.currentQuote.discount;
    });
  }
  addItem() {
    if (!this.canAdd) return;
    let item: OrderItem = {
      additionalInfo: this.currentQuote.additionalInfo.toString(),
      discount: this.currentQuote.discount,
      measureUnitId: this.currentQuote.measureUnit.measureUnitId,
      orderItemStatusId: 1,
      productId: this.currentQuote.product.productId,
      product: this.currentQuote.product.name,
      originalPrice: this.currentQuote.price,
      priceAfterDiscount: this.currentQuote.finalPrice,
      quantity: this.currentQuote.quantity,
      measureUnit: this.currentQuote.measureUnit.description,
      measureUnitShort: this.currentQuote.measureUnit.shortName,
      orderId: 0,
      orderItemId: 0,
      createdOn: null,
      lastUpdated: null
    };
    this.order.orderItems.push(item);
    this.calculateOrderValue();
  }

  open(content) {
    this.modal = this.modalService.open(content, { size: 'lg' });
  }
  applyDiscout(event) {
    this.currentQuote.finalPrice = this.currentQuote.price - this.currentQuote.discount;
  }
  changeProduct() {
    this.setMeasure();
    this.calculatePrice();
  }
  public get canAdd(): boolean {
    return this.currentQuote.quantity > 0 && this.currentQuote.discount <= this.currentQuote.price;
  }
  public onQtdKeyUp() {
    this.qtdSubject.next(this.currentQuote.quantity);
  }
  public calculateOrderValue() {
    this.order.totalPrice = this.order.freightPrice;
    this.order.orderItems.forEach((v, i) => this.order.totalPrice += v.priceAfterDiscount);
  }
  public removeItem(item) {
    this.order.orderItems = this.order.orderItems.filter((v, i) => v != item);
    this.calculateOrderValue();
  }
  resetModal() {
    this.currentQuote = { orderItemId: null, orderItemStatus: null, quantity: 0, discount: 0, finalPrice: 0, measureUnit: null, price: 0, product: this.products[0], additionalInfo: '' };
    this.setMeasure();
  }
}
