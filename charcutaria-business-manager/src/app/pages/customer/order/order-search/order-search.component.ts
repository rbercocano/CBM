import { Component, OnInit } from '@angular/core';
import { PaginationInfo } from 'src/app/shared/models/paginationInfo';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';
import { PaginationService } from 'src/app/shared/services/pagination/pagination.service';
import { NotificationService } from 'src/app/shared/services/notification/notification.service';
import { PagedResult } from 'src/app/shared/models/pagedResult';
import { OrderService } from 'src/app/shared/services/order/order.service';
import { OrderSummary } from 'src/app/shared/models/orderSummary';
import { orderSummaryFilter } from 'src/app/shared/models/orderSummaryFilter';
import { NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { OrderStatus } from 'src/app/shared/models/orderStatus';
import { PaymentStatus } from 'src/app/shared/models/paymentStatus';
import { DomainService } from 'src/app/shared/services/domain/domain.service';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-order-search',
  templateUrl: './order-search.component.html',
  styleUrls: ['./order-search.component.scss']
})
export class OrderSearchComponent implements OnInit {
  public paginationInfo: PaginationInfo = {} as PaginationInfo;
  public active: boolean = null;
  public orders: OrderSummary[] = [];
  public orderStatus: OrderStatus[] = [];
  public paymentStatus: PaymentStatus[] = [];
  public filter: orderSummaryFilter = new orderSummaryFilter();
  private lastFilter: orderSummaryFilter;
  private modal: NgbModalRef;
  constructor(private orderService: OrderService,
    private spinner: NgxSpinnerService,
    private router: Router,
    private paginationService: PaginationService,
    private modalService: NgbModal,
    private notificationService: NotificationService,
    private domainService: DomainService) {
    this.resetFilter();
    this.paginationInfo.currentPage = 1;
    this.paginationInfo.recordsPerpage = 5;
    this.paginationService.onChangePage.subscribe(r => {
      if (r != null)
        this.search();
    });
  }
  ngOnInit(): void {
    this.spinner.show();
    this.lastFilter = { ...this.filter };
    let oStatusPag = this.domainService.GetPaymentStatus();
    let oStatusOrder = this.domainService.GetOrderStatus();
    let oOrder = this.orderService.getOrderSummary(this.lastFilter, this.paginationInfo.currentPage, this.paginationInfo.recordsPerpage);
    forkJoin(oStatusPag, oStatusOrder, oOrder).subscribe(r => {
      this.spinner.hide();
      this.paymentStatus = r[0] ?? [];
      this.orderStatus = r[1] ?? [];
      let info: PagedResult<OrderSummary> = r[2] ?? { data: [], currentPage: 1, recordCount: 0, recordsPerpage: this.paginationInfo.recordsPerpage, totalPages: 0 };
      this.orders = info.data;
      this.paginationInfo = info;
      this.paginationService.updatePaging(info);
    }, (e) => {
      this.spinner.hide();
      this.notificationService.notifyHttpError(e);
    });
  }
  new() {
    this.router.navigate(['/order/new']);
  }
  details(orderNumber: number) {
    this.router.navigate(['/order/details', orderNumber]);
  }
  edit(orderNumber: number) {
    this.router.navigate(['/order/edit', orderNumber]);
  }
  newSearch() {
    this.lastFilter = { ...this.filter };
    this.paginationInfo.currentPage = 1;
    this.search();
  }
  search() {
    this.spinner.show();
    this.orders = [];
    this.orderService
      .getOrderSummary(
        this.lastFilter,
        this.paginationInfo.currentPage, this.paginationInfo.recordsPerpage)
      .subscribe(r => {
        this.spinner.hide();
        let info: PagedResult<OrderSummary> = r ?? { data: [], currentPage: 1, recordCount: 0, recordsPerpage: this.paginationInfo.recordsPerpage, totalPages: 0 };
        this.orders = info.data;
        this.paginationInfo = info;
        this.paginationService.updatePaging(info);
        this.modal.close();
      }, (e) => {
        this.spinner.hide();
        this.notificationService.notifyHttpError(e);
      });
  }
  changePageSize() {
    this.paginationService.changePageSize(this.paginationInfo.recordsPerpage);
  }
  open(content) {
    this.resetFilter();
    this.modal = this.modalService.open(content);
  }
  resetFilter() {
    this.filter = new orderSummaryFilter();
    this.filter.paymentStatus = null;
    this.filter.orderStatus = null;
  }
}
