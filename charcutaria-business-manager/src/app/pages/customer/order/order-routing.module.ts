import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NewOrderComponent } from './new-order/new-order.component';
import { OrderDetailsComponent } from './order-details/order-details.component';
import { EditOrderComponent } from './edit-order/edit-order.component';
import { OrderSearchComponent } from './order-search/order-search.component';


const routes: Routes = [{
  path: 'new',
  component: NewOrderComponent
}, {
  path: 'details/:id',
  component: OrderDetailsComponent
}, {
  path: 'edit/:id',
  component: EditOrderComponent
}, {
  path: 'search',
  component: OrderSearchComponent
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OrderRoutingModule { }
