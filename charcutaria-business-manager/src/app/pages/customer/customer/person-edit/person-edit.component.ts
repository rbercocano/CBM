import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Person } from 'src/app/shared/models/person';
import { NgxSpinnerService } from 'ngx-spinner';
import { NotificationService } from 'src/app/shared/services/notification/notification.service';
import { CustomerService } from 'src/app/shared/services/customer/customer.service';
import { Router, ActivatedRoute } from '@angular/router';
import { of, forkJoin } from 'rxjs';
import { CustomerContactComponent } from 'src/app/shared/customer-contact/customer-contact.component';
import { flatMap } from 'rxjs/operators';

@Component({
  selector: 'app-person-edit',
  templateUrl: './person-edit.component.html',
  styleUrls: ['./person-edit.component.scss']
})
export class PersonEditComponent implements OnInit, AfterViewInit {
  @ViewChild('f', { static: true }) form: NgForm;
  @ViewChild(CustomerContactComponent) customerContacts: CustomerContactComponent;

  public person: Person = {} as Person;
  public title: string;

  constructor(private spinner: NgxSpinnerService,
    private notificationService: NotificationService,
    private customerService: CustomerService,
    private router: Router,
    private route: ActivatedRoute) {
    this.person.customerId = this.route.snapshot.params.id;
    this.title = this.person.customerId ? 'Editar Cliente' : 'Cadastrar Cliente';
  }
  ngAfterViewInit(): void {
    if (!this.person.customerId) return;
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
    let oCustomer = this.person.customerId ? this.customerService.GetPerson(this.person.customerId) : of(this.person);
    oCustomer.subscribe(r => {
      this.person = r;
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
    if (!this.person.customerId)
      this.customerService
        .AddPerson(this.person)
        .pipe(flatMap(r => {
          this.person.customerId = r;
          this.customerContacts.customer = this.person;
          return this.customerContacts.init()
        }))
        .subscribe(r => {
          this.spinner.hide();
          this.notificationService.showSuccess('Sucesso', 'Cliente salvo com sucesso.');
        }, (e) => {
          this.spinner.hide();
          this.notificationService.notifyHttpError(e);
        });
    else
      this.customerService
        .UpdatePerson(this.person)
        .subscribe(r => {
          this.person = r;
          this.spinner.hide();
          this.notificationService.showSuccess('Sucesso', 'Cliente salvo com sucesso.');
        }, (e) => {
          this.spinner.hide();
          this.notificationService.notifyHttpError(e);
        });
  }
}


