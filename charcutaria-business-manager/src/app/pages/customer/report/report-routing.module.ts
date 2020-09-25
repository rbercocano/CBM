import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { OrderItemComponent } from './order-item/order-item.component';
import { SummarizedProductionComponent } from './summarized-production/summarized-production.component';


const routes: Routes = [{
  path: 'order-itens',
  component: OrderItemComponent
},
{
  path: 'production-status',
  component: SummarizedProductionComponent
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReportRoutingModule { }
