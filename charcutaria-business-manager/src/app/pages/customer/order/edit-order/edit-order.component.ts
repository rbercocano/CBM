import { Component, OnInit, ViewChild, ViewChildren, QueryList } from '@angular/core';
import { OrderDetails } from 'src/app/shared/models/orderDetails';
import { OrderService } from 'src/app/shared/services/order/order.service';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { NotificationService } from 'src/app/shared/services/notification/notification.service';
import { NgbDateStruct, NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DomainService } from 'src/app/shared/services/domain/domain.service';
import { forkJoin, Subject } from 'rxjs';
import { PaymentStatus } from 'src/app/shared/models/paymentStatus';
import { OrderStatus } from 'src/app/shared/models/orderStatus';
import { OrderItemStatus } from 'src/app/shared/models/orderItemStatus';
import { flatMap, debounceTime } from 'rxjs/operators';
import { ProductQuote } from 'src/app/shared/models/productQuote';
import { ProductService } from 'src/app/shared/services/product/product.service';
import { Product } from 'src/app/shared/models/product';
import { MeasureUnit } from 'src/app/shared/models/measureUnit';
import { PricingRequest } from 'src/app/shared/models/pricingRequest';
import { PricingService } from 'src/app/shared/services/pricing/pricing.service';
import { OrderItemDetails } from 'src/app/shared/models/orderItemDetails';
import { OrderItem } from 'src/app/shared/models/orderItem';
import { NewOrderItem } from 'src/app/shared/models/NewOrderItem';
import { IConfirmModal } from 'src/app/shared/confirm-modal/confirmModal';
import { ConfirmModalComponent } from 'src/app/shared/confirm-modal/confirm-modal.component';

@Component({
  selector: 'app-edit-order',
  templateUrl: './edit-order.component.html',
  styleUrls: ['./edit-order.component.scss']
})
export class EditOrderComponent implements OnInit {
  public title = 'Detalhes do Pedido';
  @ViewChild("close", { static: false }) cClose: ConfirmModalComponent;
  @ViewChild("restore", { static: false }) cRestore: ConfirmModalComponent;
  @ViewChild("cancelorder", { static: false }) cCancelOrder: ConfirmModalComponent;
  public order: OrderDetails;
  public minDate: NgbDateStruct;
  public paymentStatus: PaymentStatus[] = [];
  public orderStatus: OrderStatus[] = [];
  public orderItemStatus: OrderItemStatus[] = [];
  public products: Product[] = [];
  public measures: MeasureUnit[] = [];
  private modal: NgbModalRef;
  private lastStatus: { paymentStatusId, orderStatusId } = null;
  public currentQuote: ProductQuote = { orderItemId: null, orderItemStatus: null, quantity: 0, discount: 0, finalPrice: 0, measureUnit: null, price: 0, product: null, additionalInfo: '' };
  private qtdSubject: Subject<number> = new Subject();
  public canEditItem = true;
  constructor(private orderService: OrderService,
    private route: ActivatedRoute,
    private router: Router,
    private spinner: NgxSpinnerService,
    private notificationService: NotificationService,
    private modalService: NgbModal,
    private domainService: DomainService,
    private productService: ProductService,
    private pricingService: PricingService) {
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
    let orderNumber = this.route.snapshot.params.id;
    let oPaymentStatus = this.domainService.GetPaymentStatus();
    let oOrderStatus = this.domainService.GetOrderStatus();
    let oOrderItemStatus = this.domainService.GetOrderItemStatus();
    let oOrder = this.orderService.getByNumber(orderNumber);
    let oMeasures = this.domainService.GetMeasureUnits();
    let oProducts = this.productService.GetAll();
    forkJoin(oPaymentStatus, oOrderStatus, oOrderItemStatus, oOrder, oMeasures, oProducts).subscribe(r => {
      this.paymentStatus = r[0] ?? [];
      this.orderStatus = r[1] ?? [];
      this.orderItemStatus = r[2] ?? [];
      this.setOrder(r[3]);
      this.measures = r[4] ?? [];
      this.products = r[5] ?? [];
      if (this.products.length > 0) {
        this.currentQuote.product = this.products[0];
        this.setMeasure();
      }
      if (this.measures.length > 0) {
        this.currentQuote.measureUnit = this.measures.filter((v, i) => v.measureUnitId == this.currentQuote.product.measureUnitId)[0];
      }
      this.spinner.hide();
    }, e => {
      this.spinner.hide();
      this.notificationService.notifyHttpError(e);
    });
  }
  private setOrder(result: OrderDetails) {
    this.order = result;
    if (result == null) return;
    this.lastStatus = {
      orderStatusId: this.order.orderStatusId,
      paymentStatusId: this.order.paymentStatusId
    };
    this.setOrderStatuses();
    this.spinner.hide();
  }
  private setOrderStatuses() {
    this.order.paymentStatus = this.paymentStatus.filter((v, i) => v.paymentStatusId == this.order.paymentStatusId)[0];
    this.order.orderStatus = this.orderStatus.filter((v, i) => v.orderStatusId == this.order.orderStatusId)[0];
  }
  save() {
    if (this.order.completeBy == null) return;
    this.spinner.show();
    this.orderService.update({
      completeBy: this.order.completeBy,
      freightPrice: this.order.freightPrice,
      orderNumber: this.order.orderNumber,
      paymentStatusId: this.order.paymentStatus.paymentStatusId
    }).pipe(flatMap(r => {
      return this.orderService.getByNumber(this.order.orderNumber)
    })).subscribe(r => {
      this.setOrder(r);
      this.spinner.hide();
      this.notificationService.showSuccess('Sucesso', 'Pedido salvo com sucesso.');
    }, e => {
      this.spinner.hide();
      this.notificationService.notifyHttpError(e);
    });
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
  public calculateOrderValue() {
    this.order.orderTotal = this.order.itemsTotalAfterDiscounts + this.order.freightPrice;
  }
  cancelOrder(orderNumber: any) {
    this.cCancelOrder.close();
    this.spinner.show();
    this.orderService.cancel(orderNumber).pipe(flatMap(r => {
      return this.orderService.getByNumber(orderNumber)
    })).subscribe(r => {
      this.setOrder(r);
      this.spinner.hide();
      this.notificationService.showSuccess('Sucesso', 'Pedido cancelado com sucesso.');
    }, e => {
      this.spinner.hide();
      this.notificationService.notifyHttpError(e);
    });
  }
  public confirmClose() {
    this.cClose.open();
  }
  public confirmRestore() {
    this.cRestore.open();
  }
  public confirmCancelOrder() {
    this.cCancelOrder.open();
  }
  public closeOrder(orderNumber: any) {
    this.cClose.close();
    this.spinner.show();
    this.orderService.close(orderNumber).pipe(flatMap(r => {
      return this.orderService.getByNumber(orderNumber)
    })).subscribe(r => {
      this.setOrder(r);
      this.spinner.hide();
      this.notificationService.showSuccess('Sucesso', 'Ordem restaurada com sucesso.');
    }, e => {
      this.spinner.hide();
      this.notificationService.notifyHttpError(e);
    });
  }
  restoreOrder(orderNumber: any) {
    this.cRestore.close();
    this.spinner.show();
    this.orderService.restore(orderNumber).pipe(flatMap(r => {
      return this.orderService.getByNumber(orderNumber)
    })).subscribe(r => {
      this.setOrder(r);
      this.spinner.hide();
      this.notificationService.showSuccess('Sucesso', 'Ordem restaurada com sucesso.');
    }, e => {
      this.spinner.hide();
      this.notificationService.notifyHttpError(e);
    });
  }

  open(content, orderItem: OrderItemDetails) {
    this.canEditItem = orderItem == null ? true :
      this.allowUpdateStatus && orderItem.orderItemStatus.orderItemStatusId != 5;
    if (orderItem == null)
      this.resetModal();
    else {
      this.currentQuote = {
        quantity: orderItem.quantity,
        discount: orderItem.discount,
        finalPrice: orderItem.priceAfterDiscount,
        measureUnit: this.measures.filter(p => p.measureUnitId == orderItem.measureUnit.measureUnitId)[0],
        price: orderItem.product.price,
        product: this.products.filter(p => p.productId == orderItem.product.productId)[0],
        additionalInfo: orderItem.additionalInfo,
        orderItemId: orderItem.orderItemId,
        orderItemStatus: this.orderItemStatus.filter(s => s.orderItemStatusId == orderItem.orderItemStatus.orderItemStatusId)[0]
      };
    }
    this.modal = this.modalService.open(content, { size: 'lg' });
  }
  private setMeasure() {
    this.currentQuote.measureUnit = this.currentQuote.product == null ?
      this.measures[0] : this.measures.filter((v, i) => v.measureUnitId == this.currentQuote.product.measureUnitId)[0];
  }
  public onQtdKeyUp() {
    this.qtdSubject.next(this.currentQuote.quantity);
  }
  public applyDiscout(event) {
    this.currentQuote.finalPrice = this.currentQuote.price - this.currentQuote.discount;
  }
  resetModal() {
    this.currentQuote = { orderItemId: null, orderItemStatus: this.orderItemStatus[0], quantity: 0, discount: 0, finalPrice: 0, measureUnit: null, price: 0, product: this.products[0], additionalInfo: '' };
    this.setMeasure();
  }
  public get allowEdit(): boolean {
    if (this.lastStatus == null) return false;
    return (this.lastStatus.paymentStatusId == 1
      && this.lastStatus.orderStatusId != 4);
  }
  public get allowUpdateStatus(): boolean {
    if (this.lastStatus == null) return false;
    return (this.lastStatus.orderStatusId != 4);
  }
  public addItem() {
    this.spinner.show();
    let item: NewOrderItem = {
      additionalInfo: this.currentQuote.additionalInfo,
      discount: this.currentQuote.discount,
      measureUnitId: this.currentQuote.measureUnit.measureUnitId,
      orderId: this.order.orderId,
      orderItemId: null,
      orderItemStatusId: this.currentQuote.orderItemStatus.orderItemStatusId,
      productId: this.currentQuote.product.productId,
      quantity: this.currentQuote.quantity
    };
    this.orderService.addOrderItem(item).pipe(flatMap(r => {
      return this.orderService.getByNumber(this.order.orderNumber)
    })).subscribe(r => {
      this.setOrder(r);
      this.spinner.hide();
      this.notificationService.showSuccess('Sucesso', 'Item adicionado com sucesso.');
    }, e => {
      this.spinner.hide();
      this.notificationService.notifyHttpError(e);
    });

  }
  public saveItem() {
    this.modal.close();
    this.spinner.show();
    let item: NewOrderItem = {
      additionalInfo: this.currentQuote.additionalInfo,
      discount: this.currentQuote.discount,
      measureUnitId: this.currentQuote.measureUnit.measureUnitId,
      orderId: this.order.orderId,
      orderItemId: this.currentQuote.orderItemId,
      orderItemStatusId: this.currentQuote.orderItemStatus.orderItemStatusId,
      productId: this.currentQuote.product.productId,
      quantity: this.currentQuote.quantity
    };
    this.orderService.updateOrderItem(item).pipe(flatMap(r => {
      return this.orderService.getByNumber(this.order.orderNumber)
    })).subscribe(r => {
      this.setOrder(r);
      this.spinner.hide();
      this.notificationService.showSuccess('Sucesso', 'Item atualizado com sucesso.');
    }, e => {
      this.spinner.hide();
      this.notificationService.notifyHttpError(e);
    });

  }
  public removeItem(item: OrderItem) {
    this.spinner.show();
    this.orderService.removeOrderItem(this.order.orderId, item.orderItemId).pipe(flatMap(r => {
      return this.orderService.getByNumber(this.order.orderNumber)
    })).subscribe(r => {
      this.setOrder(r);
      this.spinner.hide();
      this.notificationService.showSuccess('Sucesso', 'Item removido com sucesso.');
    }, e => {
      this.spinner.hide();
      this.notificationService.notifyHttpError(e);
    });
  }
  public changeProduct() {
    this.setMeasure();
    this.calculatePrice();
  }
  public goToDetails() {
    this.router.navigate(['/order/details', this.order.orderNumber]);
  }
}
