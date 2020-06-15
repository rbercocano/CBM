import { Component, OnInit } from '@angular/core';
import { OrderItemReport } from 'src/app/shared/models/orderItemReport';
import { OrderItemReportFilter } from 'src/app/shared/models/OrderItemReportFilter';
import { NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PaginationInfo } from 'src/app/shared/models/paginationInfo';
import { OrderStatus } from 'src/app/shared/models/orderStatus';
import { OrderItemStatus } from 'src/app/shared/models/orderItemStatus';
import { OrderService } from 'src/app/shared/services/order/order.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';
import { PaginationService } from 'src/app/shared/services/pagination/pagination.service';
import { NotificationService } from 'src/app/shared/services/notification/notification.service';
import { DomainService } from 'src/app/shared/services/domain/domain.service';
import { forkJoin } from 'rxjs';
import { PagedResult } from 'src/app/shared/models/pagedResult';
import { IDropdownSettings } from 'ng-multiselect-dropdown';

@Component({
  selector: 'app-order-item',
  templateUrl: './order-item.component.html',
  styleUrls: ['./order-item.component.scss']
})
export class OrderItemComponent implements OnInit {
  public paginationInfo: PaginationInfo = {} as PaginationInfo;
  public active: boolean = null;
  public items: OrderItemReport[] = [];
  public orderStatus: OrderStatus[] = [];
  public orderItemStatus: OrderItemStatus[] = [];
  public filter: OrderItemReportFilter = new OrderItemReportFilter();
  private lastFilter: OrderItemReportFilter;
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
  public orderItemStatusSettings: IDropdownSettings = {
    singleSelection: false,
    idField: 'orderItemStatusId',
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
    let oOrderItemStatus = this.domainService.GetOrderItemStatus();
    let oStatusOrder = this.domainService.GetOrderStatus();
    let oOrder = this.orderService.getOrderItemReport(this.lastFilter, this.paginationInfo.currentPage, this.paginationInfo.recordsPerpage);
    forkJoin(oOrderItemStatus, oStatusOrder, oOrder).subscribe(r => {
      this.spinner.hide();
      this.orderItemStatus = r[0] ?? [];
      this.orderStatus = r[1] ?? [];
      let info: PagedResult<OrderItemReport> = r[2] ?? { data: [], currentPage: 1, recordCount: 0, recordsPerpage: this.paginationInfo.recordsPerpage, totalPages: 0 };
      this.items = info.data;
      this.paginationInfo = info;
      this.paginationService.updatePaging(info);
    }, (e) => {
      this.spinner.hide();
      this.notificationService.notifyHttpError(e);
    });
  }
  newSearch() {
    this.filter.direction = null;
    this.filter.orderBy = null;
    this.lastFilter = { ...this.filter };
    this.paginationInfo.currentPage = 1;
    this.search();
  }
  search() {
    this.spinner.show();
    this.items = [];
    if (this.modal)
      this.modal.close();
    this.orderService
      .getOrderItemReport(
        this.lastFilter,
        this.paginationInfo.currentPage, this.paginationInfo.recordsPerpage)
      .subscribe(r => {
        this.spinner.hide();
        let info: PagedResult<OrderItemReport> = r ?? { data: [], currentPage: 1, recordCount: 0, recordsPerpage: this.paginationInfo.recordsPerpage, totalPages: 0 };
        this.items = info.data;
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
    this.modal = this.modalService.open(content, { size: 'lg' });
  }
  resetFilter() {
    this.filter = new OrderItemReportFilter();
    this.filter.orderBy = null;
    this.filter.direction = null;
    this.filter.itemStatus = null;
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
