import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { OrderItemComponent } from './order-item/order-item.component';
import { StatusReportComponent } from './status-report/status-report.component';
import { SummarizedProductionComponent } from './summarized-production/summarized-production.component';


const routes: Routes = [{
  path: 'status-report',
  component: StatusReportComponent
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReportRoutingModule { }
