import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NewOrderComponent } from './new-order/new-order.component';
import { OrderDetailsComponent } from './order-details/order-details.component';


const routes: Routes = [{
  path: 'new',
  component: NewOrderComponent
}, {
  path: 'details/:id',
  component: OrderDetailsComponent
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OrderRoutingModule { }
