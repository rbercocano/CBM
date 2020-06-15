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
import { IDropdownSettings } from 'ng-multiselect-dropdown';

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

  public orderStatusSettings: IDropdownSettings = {
    singleSelection: false,
    idField: 'orderStatusId',
    textField: 'description',
    selectAllText: 'Selecionar Todos',
    unSelectAllText: 'Deselecionar Todos',
    searchPlaceholderText: 'Pesquisar',
    noDataAvailablePlaceholderText: 'Nenhuma opção disponível',
    itemsShowLimit: 2,
    allowSearchFilter: true
  };
  public paymentStatusSettings: IDropdownSettings = {
    singleSelection: false,
    idField: 'paymentStatusId',
    textField: 'description',
    selectAllText: 'Selecionar Todos',
    unSelectAllText: 'Deselecionar Todos',
    searchPlaceholderText: 'Pesquisar',
    noDataAvailablePlaceholderText: 'Nenhuma opção disponível',
    itemsShowLimit: 2,
    allowSearchFilter: true
  };
  constructor(private orderService: OrderService,
    private spinner: NgxSpinnerService,
    private router: Router,
    private paginationService: PaginationService,
    private modalService: NgbModal,
    private notificationService: NotificationService,
    private domainService: DomainService) {
    this.resetFilter();
    this.lastFilter = { ...this.filter };
    this.paginationInfo.currentPage = 1;
    this.paginationInfo.recordsPerpage = 10;
    this.paginationService.onChangePage.subscribe(r => {
      if (r != null)
        this.search();
    });
  }
  onSelect(e: any) {
    console.log(e);
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
    this.filter.direction = 1;
    this.filter.orderBy = 1;
    this.lastFilter = { ...this.filter };
    this.paginationInfo.currentPage = 1;
    this.search();
  }
  search() {
    this.spinner.show();
    this.orders = [];
    if (this.modal)
      this.modal.close();
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
    this.modal = this.modalService.open(content,{size:'lg'});
  }
  resetFilter() {
    this.filter = new orderSummaryFilter();
    this.filter.paymentStatus = null;
    this.filter.orderStatus = null;
  }
  sort(column: number) {
    if (this.lastFilter.orderBy == column) {
      this.lastFilter.direction = this.lastFilter.direction == 1 ? 2 : 1;
    } else {
      this.lastFilter.orderBy = column;
      this.lastFilter.direction = 1;
    }
    this.search();
  }
  public get sortColumn(): number {
    return this.lastFilter.orderBy;
  }
  public get sortDirection(): number {
    return this.lastFilter.direction;
  }
}
