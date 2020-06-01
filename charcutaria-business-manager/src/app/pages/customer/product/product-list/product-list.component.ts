import { Component, OnInit } from '@angular/core';
import { ProductService } from 'src/app/shared/services/product/product.service';
import { Product } from 'src/app/shared/models/product';
import { NgxSpinnerService } from "ngx-spinner";
import { Router } from '@angular/router';
import { PaginationService } from 'src/app/shared/services/pagination/pagination.service';
import { PaginationInfo } from 'src/app/shared/models/paginationInfo';
import { PagedResult } from 'src/app/shared/models/pagedResult';
import { NotificationService } from 'src/app/shared/services/notification/notification.service';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.scss']
})
export class ProductListComponent implements OnInit {
  public paginationInfo: PaginationInfo = {} as PaginationInfo;
  public filter = "";
  public active: boolean = null;
  public products: Product[] = [];

  constructor(private productService: ProductService,
    private spinner: NgxSpinnerService,
    private router: Router,
    private paginationService: PaginationService,
    private notificationService: NotificationService) {
    this.paginationInfo.currentPage = 1;
    this.paginationInfo.recordsPerpage = 10;
    this.paginationService.onChangePage.subscribe(r => {
      if (r != null)
        this.search();
    });
  }
  ngOnInit(): void {
    this.search();
  }
  new() {
    this.router.navigate(['/product/new']);
  }
  edit(id: number) {
    this.router.navigate(['/product/edit', id]);
  }
  newSearch() {
    this.paginationInfo.currentPage = 1;
    this.search();
  }
  search() {
    this.spinner.show();
    this.products = [];
    this.productService
      .GetPaged(this.paginationInfo.currentPage, this.paginationInfo.recordsPerpage, this.filter, this.active)
      .subscribe(r => {
        let info: PagedResult<Product> = r ?? { data: [], currentPage: 1, recordCount: 0, recordsPerpage: this.paginationInfo.recordsPerpage, totalPages: 0 };
        this.products = info.data;
        this.paginationInfo = info;
        this.paginationService.updatePaging(info);
        this.spinner.hide();
      }, (e) => {
        this.spinner.hide();
        this.notificationService.notifyHttpError(e);
      });
  }
  changePageSize() {
    this.paginationService.changePageSize(this.paginationInfo.recordsPerpage);
  }
  datasheet(productId: number) {
    this.router.navigate(['/product', productId, 'datasheet']);
  }
  datasheetDetails(productId: number) {
    this.router.navigate(['/product', productId, 'datasheet','details']);
  }
}
