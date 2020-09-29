import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { NgxSpinnerService } from 'ngx-spinner';
import { forkJoin } from 'rxjs';
import { MeasureUnit } from 'src/app/shared/models/measureUnit';
import { OrderItemReportFilter } from 'src/app/shared/models/OrderItemReportFilter';
import { OrderItemStatus } from 'src/app/shared/models/orderItemStatus';
import { PagedResult } from 'src/app/shared/models/pagedResult';
import { PaginationInfo } from 'src/app/shared/models/paginationInfo';
import { Product } from 'src/app/shared/models/product';
import { SummarizedOrderReport } from 'src/app/shared/models/summarizedOrderReport';
import { SummarizedOrderReportVM } from 'src/app/shared/models/summarizedOrderReportVM';
import { SummarizedProductionFilter } from 'src/app/shared/models/summarizedProductionFilter';
import { DomainService } from 'src/app/shared/services/domain/domain.service';
import { NotificationService } from 'src/app/shared/services/notification/notification.service';
import { OrderService } from 'src/app/shared/services/order/order.service';
import { PaginationService } from 'src/app/shared/services/pagination/pagination.service';
import { ProductService } from 'src/app/shared/services/product/product.service';

@Component({
  selector: 'app-summarized-production',
  templateUrl: './summarized-production.component.html',
  styleUrls: ['./summarized-production.component.scss']
})
export class SummarizedProductionComponent implements OnInit {
  private modal: NgbModalRef;
  public filter: SummarizedProductionFilter = new SummarizedProductionFilter();
  private lastFilter: SummarizedProductionFilter;
  public paginationInfo: PaginationInfo = {} as PaginationInfo;
  public items: SummarizedOrderReportVM[] = [];
  public products: Product[] = [];
  public itemStatus: OrderItemStatus[] = [];
  public mass: MeasureUnit[] = [];
  public volumes: MeasureUnit[] = [];
  constructor(private orderService: OrderService,
    private spinner: NgxSpinnerService,
    private router: Router,
    private productService: ProductService,
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
  public productSettings: IDropdownSettings = {
    singleSelection: false,
    idField: 'productId',
    textField: 'name',
    selectAllText: 'Selecionar Todos',
    unSelectAllText: 'Deselecionar Todos',
    searchPlaceholderText: 'Pesquisar',
    noDataAvailablePlaceholderText: 'Nenhuma opção disponível',
    itemsShowLimit: 2,
    allowSearchFilter: true
  };
  ngOnInit(): void {
    this.spinner.show();
    let oStatus = this.domainService.GetOrderItemStatus();
    let oProducts = this.productService.GetPaged(1, 1000, null, true);
    let oMeasures = this.domainService.GetMeasureUnits();
    let oItems = this.orderService.getSummarizedReport(this.lastFilter, this.paginationInfo.currentPage, this.paginationInfo.recordsPerpage);
    forkJoin([oStatus, oProducts, oItems, oMeasures]).subscribe(r => {
      this.spinner.hide();
      this.itemStatus = r[0] ?? [];
      let prodInfo: PagedResult<Product> = r[1] ?? { data: [], currentPage: 1, recordCount: 0, recordsPerpage: this.paginationInfo.recordsPerpage, totalPages: 0 };
      this.products = prodInfo.data;
      let info: PagedResult<SummarizedOrderReport> = r[2] ?? { data: [], currentPage: 1, recordCount: 0, recordsPerpage: this.paginationInfo.recordsPerpage, totalPages: 0 };
      this.items = info.data as SummarizedOrderReportVM[];
      let measures = r[3] ?? [];
      measures.forEach(m => {
        if (m.measureUnitTypeId == 1)
          this.mass.push(m);
        else if (m.measureUnitTypeId == 2)
          this.volumes.push(m);
      });
      this.paginationInfo = info;
      this.paginationService.updatePaging(info);
    }, (e) => {
      this.spinner.hide();
      this.notificationService.notifyHttpError(e);
    });
  }
  search() {
    this.spinner.show();
    this.items = [];
    if (this.modal)
      this.modal.close();
    this.orderService
      .getSummarizedReport(
        this.lastFilter,
        this.paginationInfo.currentPage, this.paginationInfo.recordsPerpage)
      .subscribe(r => {
        this.spinner.hide();
        let info: PagedResult<SummarizedOrderReport> = r ?? { data: [], currentPage: 1, recordCount: 0, recordsPerpage: this.paginationInfo.recordsPerpage, totalPages: 0 };
        this.items = info.data as SummarizedOrderReportVM[];
        this.paginationInfo = info;
        this.paginationService.updatePaging(info);
        this.filter = { ...this.lastFilter };
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
  changePageSize() {
    this.paginationService.changePageSize(this.paginationInfo.recordsPerpage);
  }
  open(content) {
    // this.resetFilter();
    this.modal = this.modalService.open(content, { size: 'lg' });
  }
  resetFilter() {
    this.filter = new SummarizedProductionFilter();
    this.filter.orderBy = null;
    this.filter.direction = null;
    this.filter.itemStatus = null;
    this.filter.massUnitId = 1;
    this.filter.volumeUnitId = 5;
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
  toggle(item: SummarizedOrderReportVM) {
    item.isExpanded = item.isExpanded ?? false;
    item.orders = item.orders ?? [];
    if (!item.isExpanded && item.orders.length == 0) {
      this.spinner.show();
      let filter = new OrderItemReportFilter();
      filter.volumeUnitId = this.lastFilter.volumeUnitId;
      filter.massUnitId = this.lastFilter.massUnitId;
      filter.products = [item.productId];
      filter.itemStatus = [{ orderItemStatusId: item.orderItemStatusId, description: item.orderItemStatus }];
      this.orderService.getOrderItemReport(filter, 1, 50).subscribe(r => {
        item.orders = r.data ?? [];
        item.isExpanded = !item.isExpanded;
        this.spinner.hide();
      }, (e) => {
        this.spinner.hide();
        this.notificationService.notifyHttpError(e);
      });
    } else {
      item.isExpanded = !item.isExpanded;
    }
  }
}
