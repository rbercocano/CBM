import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ClientRegistration } from 'src/app/shared/models/clientRegistration';
import { CustomerType } from 'src/app/shared/models/customerType';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {
  @ViewChild('f', { static: true }) form: NgForm;
  public client: ClientRegistration = {} as ClientRegistration;
  public customerTypes: CustomerType[] = [{ description: 'Pessoa Física', customerTypeId: 1 }, { description: 'Pessoa Jurídica', customerTypeId: 2 }];
  constructor() { }

  ngOnInit(): void {
    this.client.customerTypeId = 2;
  }
  public signUp(): void {
    if (this.form.invalid) return;

  }
}
