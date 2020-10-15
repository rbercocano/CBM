import { Component, OnInit, ViewChild } from '@angular/core';
import { OrderItemComponent } from '../order-item/order-item.component';
import { SummarizedProductionComponent } from '../summarized-production/summarized-production.component';

@Component({
  selector: 'app-status-report',
  templateUrl: './status-report.component.html',
  styleUrls: ['./status-report.component.scss']
})
export class StatusReportComponent implements OnInit {
  @ViewChild(OrderItemComponent) orderItemComponent: OrderItemComponent;
  @ViewChild(SummarizedProductionComponent) summarizedProductionComponent: SummarizedProductionComponent;
  public view: number = 1;
  constructor() { }

  ngOnInit(): void {

  }
  public open(): void {
    if (this.view == 1)
      this.summarizedProductionComponent.open();
    else
      this.orderItemComponent.open();
  }
  public search(): void {
    if (this.view == 1)
      this.summarizedProductionComponent.search();
    else
      this.orderItemComponent.search();
  }
  public get title(): string {
    return this.view == 1 ? 'Status Report - Analítico' : 'Status Report - Sintético';
  }
}
