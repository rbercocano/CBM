import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CustomerRoutingModule } from './customer-routing.module';
import { CustomerListComponent } from './customer-list/customer-list.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms'; 
import { PersonEditComponent } from './person-edit/person-edit.component';
import { CompanyEditComponent } from './company-edit/company-edit.component';


@NgModule({
  declarations: [CustomerListComponent, PersonEditComponent, CompanyEditComponent],
  imports: [
    CommonModule,
    CustomerRoutingModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule
  ]
})
export class CustomerModule { }
