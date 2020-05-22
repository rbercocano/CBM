import { Component, OnInit } from '@angular/core';
import { PaginationInfo } from 'src/app/shared/models/paginationInfo';
import { DomainService } from 'src/app/shared/services/domain/domain.service';
import { CustomerService } from 'src/app/shared/services/customer/customer.service';
import { PaginationService } from 'src/app/shared/services/pagination/pagination.service';
import { PagedResult } from 'src/app/shared/models/pagedResult';
import { Person } from 'src/app/shared/models/person';
import { Company } from 'src/app/shared/models/company';
import { NgxSpinnerService } from "ngx-spinner";
import { forkJoin } from 'rxjs';
import { NotificationService } from 'src/app/shared/services/notification/notification.service';
import { CustomerType } from 'src/app/shared/models/customerType';
import { Router } from '@angular/router';
import { Customer } from 'src/app/shared/models/customerBase';

@Component({
  selector: 'app-customer-list',
  templateUrl: './customer-list.component.html',
  styleUrls: ['./customer-list.component.scss']
})
export class CustomerListComponent implements OnInit {
  public paginationInfo: PaginationInfo = {} as PaginationInfo;
  public filter = "";
  public customerTypeId = 1;
  public people: Person[] = [];
  public companies: Company[] = [];
  public customerTypes: CustomerType[] = [];
  private customerTypeIdSearch: number;
  public selectedCustomer: Customer;

  constructor(private domainService: DomainService,
    private router: Router,
    private spinner: NgxSpinnerService,
    private customerService: CustomerService,
    private paginationService: PaginationService,
    private notificationService: NotificationService) {
    this.paginationInfo.currentPage = 1;
    this.paginationInfo.recordsPerpage = 5;
    this.paginationService.onChangePage.subscribe(r => {
      if (r != null)
        this.search();
    });
  }

  ngOnInit(): void {
    this.spinner.show();
    let oDomain = this.domainService.GetCustomerTypes();
    this.customerTypeIdSearch = this.customerTypeId;
    let oCustomers = this.customerTypeId == 1 ?
      this.customerService.GetPagedPerson(this.paginationInfo.currentPage, this.paginationInfo.recordsPerpage, this.filter) :
      this.customerService.GetPagedCompany(this.paginationInfo.currentPage, this.paginationInfo.recordsPerpage, this.filter);
    forkJoin(oDomain, oCustomers).subscribe(r => {
      this.customerTypes = r[0];
      this.handleSearchResult(r[1]);
    }, (e) => {
      this.spinner.hide();
      this.notificationService.notifyHttpError(e);
    });
  }
  search() {
    this.selectedCustomer = null;
    this.customerTypeIdSearch = this.customerTypeId;
    this.people = [];
    this.companies = [];
    this.spinner.show();
    if (this.customerTypeIdSearch == 1)
      this.customerService.GetPagedPerson(this.paginationInfo.currentPage,
        this.paginationInfo.recordsPerpage,
        this.filter).subscribe(r => { this.handleSearchResult(r); });
    else
      this.customerService.GetPagedCompany(this.paginationInfo.currentPage,
        this.paginationInfo.recordsPerpage,
        this.filter).subscribe(r => { this.handleSearchResult(r); });

  }
  handleSearchResult(result: PagedResult<Company> | PagedResult<Person>) {
    let info: PagedResult<any> = result ?? { data: [], currentPage: 1, recordCount: 0, recordsPerpage: this.paginationInfo.recordsPerpage, totalPages: 0 };
    if (this.customerTypeIdSearch == 1)
      this.people = info.data;
    else
      this.companies = info.data;
    this.paginationInfo = info;
    this.paginationService.updatePaging(info);
    this.spinner.hide();
  }

  changePageSize() {
    this.paginationService.changePageSize(this.paginationInfo.recordsPerpage);
  }
  newPerson() {
    this.router.navigate(['/customer/person/new']);
  }
  newCompany() {
    this.router.navigate(['/customer/company/new']);
  }
  edit(id: number) {
    if (this.customerTypeIdSearch == 1)
      this.router.navigate(['/customer/person/edit', id]);
    else
      this.router.navigate(['/customer/company/edit', id]);
  }
  newSearch() {
    this.paginationInfo.currentPage = 1;
    this.search();
  }
}
