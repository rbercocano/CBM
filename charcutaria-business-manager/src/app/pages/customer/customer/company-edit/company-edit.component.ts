import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Company } from 'src/app/shared/models/company';
import { NgxSpinnerService } from 'ngx-spinner';
import { NotificationService } from 'src/app/shared/services/notification/notification.service';
import { CustomerService } from 'src/app/shared/services/customer/customer.service';
import { Router, ActivatedRoute } from '@angular/router';
import { of, forkJoin } from 'rxjs';
import { CustomerContactComponent } from 'src/app/shared/customer-contact/customer-contact.component';
import { map, flatMap } from 'rxjs/operators';

@Component({
  selector: 'app-company-edit',
  templateUrl: './company-edit.component.html',
  styleUrls: ['./company-edit.component.scss']
})
export class CompanyEditComponent implements OnInit, AfterViewInit {
  @ViewChild('f', { static: true }) form: NgForm;
  @ViewChild(CustomerContactComponent) customerContacts: CustomerContactComponent;

  public company: Company = {} as Company;
  public title: string;

  constructor(private spinner: NgxSpinnerService,
    private notificationService: NotificationService,
    private customerService: CustomerService,
    private router: Router,
    private route: ActivatedRoute) {
    this.company.customerId = this.route.snapshot.params.id;
    this.title = this.company.customerId ? 'Editar Cliente' : 'Cadastrar Cliente';
  }
  ngAfterViewInit(): void {
    if (!this.company.customerId) return;
    this.spinner.show();
    this.customerContacts.init().subscribe(r => {
      this.spinner.hide();
    }, e => {
      this.spinner.hide();
      this.notificationService.notifyHttpError(e);
    });
  }

  ngOnInit(): void {
    this.spinner.show();
    let oCustomer = this.company.customerId ? this.customerService.GetCompany(this.company.customerId) : of(this.company);
    oCustomer.subscribe(r => {
      this.company = r;
      this.spinner.hide();
    }, e => {
      this.spinner.hide();
      this.notificationService.notifyHttpError(e);
    });
  }
  back() {
    this.router.navigate(['/customer']);
  }
  save() {
    if (!this.form.valid) return;
    this.spinner.show();
    if (!this.company.customerId)
      this.customerService
        .AddCompany(this.company)
        .pipe(flatMap(r => {
          this.company.customerId = r;
          this.customerContacts.customer = this.company;
          return this.customerContacts.init()
        })).subscribe(r => {
          this.spinner.hide();
          this.notificationService.showSuccess('Sucesso', 'Cliente salvo com sucesso.');
        }, (e) => {
          this.spinner.hide();
          this.notificationService.notifyHttpError(e);
        });
    else
      this.customerService
        .UpdateCompany(this.company)
        .subscribe(r => {
          this.company = r;
          this.spinner.hide();
          this.notificationService.showSuccess('Sucesso', 'Cliente salvo com sucesso.');
        }, (e) => {
          this.spinner.hide();
          this.notificationService.notifyHttpError(e);
        });
  }
}


