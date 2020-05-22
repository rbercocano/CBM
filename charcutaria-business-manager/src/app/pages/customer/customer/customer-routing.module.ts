import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CustomerListComponent } from './customer-list/customer-list.component';
import { PersonEditComponent } from './person-edit/person-edit.component';
import { CompanyEditComponent } from './company-edit/company-edit.component';


const routes: Routes = [{
  path: '',
  component: CustomerListComponent
}, {
  path: 'person/new',
  component: PersonEditComponent
}, {
  path: 'person/edit/:id',
  component: PersonEditComponent
}, {
  path: 'company/new',
  component: CompanyEditComponent
}, {
  path: 'company/edit/:id',
  component: CompanyEditComponent
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CustomerRoutingModule { }
