import { Component, OnInit } from '@angular/core';
import { OrderCountSummary } from 'src/app/shared/models/orderCountSummary';
import { PendingPaymentsSummary } from 'src/app/shared/models/pendingPaymentSummary';
import { SalesSummary } from 'src/app/shared/models/salesSummary';
import { ProfitSummary } from 'src/app/shared/models/profitSummary';
import { OrderService } from 'src/app/shared/services/order/order.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { NotificationService } from 'src/app/shared/services/notification/notification.service';
import { forkJoin } from 'rxjs';
import 'd3';
import * as c3 from 'c3';
import { ProductService } from 'src/app/shared/services/product/product.service';
import { ProductCostProfit } from 'src/app/shared/models/productCostProfit';
import { Production } from 'src/app/shared/models/production';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss',
    '../../../../../../node_modules/c3/c3.min.css']
})
export class DashboardComponent implements OnInit {
  public orderCountSummary: OrderCountSummary = { rowId: '', totalCompletedOrders: 0, totalOrders: 0 };
  public pendingPaymentsSummary: PendingPaymentsSummary = { rowId: '', totalPendingPayments: 0, finishedOrdersPendingPayment: 0 };
  public salesSummary: SalesSummary = { rowId: '', totalSales: 0, currentMonthSales: 0 };
  public profitSummary: ProfitSummary = { rowId: '', totalProfit: 0, currentMonthProfit: 0 };
  public profitAndCost: ProductCostProfit[] = [];
  public production: Production[] = [];
  constructor(private orderService: OrderService,
    private productService: ProductService,
    private spinner: NgxSpinnerService,
    private notificationService: NotificationService, ) { }

  ngOnInit(): void {

    this.spinner.show();
    forkJoin(
      this.orderService.getOrderCountSummary(),
      this.orderService.getPendingPaymentsSummary(),
      this.orderService.getSalesSummary(),
      this.orderService.getProfitSummary(),
      this.productService.getCostProfit(),
      this.productService.getProduction()
    ).subscribe(r => {
      this.orderCountSummary = r[0];
      this.pendingPaymentsSummary = r[1];
      this.salesSummary = r[2];
      this.profitSummary = r[3];
      this.profitAndCost = (r[4] ?? []).slice(0, 5);
      this.production = (r[5] ?? []).slice(0, 5);
    }, e => {
      this.notificationService.notifyHttpError(e);
      this.spinner.hide();
    }, () => {
      this.spinner.hide();
    })
  }
  private getColorArray(qtd: number): Array<string> {
    let colors = [
      '#6666ff', //azul
      '#ff3333', //vermelho
      '#00b300', //verde
      '#ff9900', //Laranja
      '#660066', //Roxo
      '#ffcb80', //Amarelo
      '#ff869a', //Rosa
      '#2ed8b6', //Verde Claro
      '#4099ff', //Azul Claro
      '#666699' //Roxo fosco
    ];
    let result = [];
    for (let index = 0; index < qtd; index++) {
      if (index < colors.length)
        result.push(colors[index]);
      else {
        while (true) {
          let random = Math.floor(Math.random() * 16777215).toString(16);
          let existingColor = colors.filter(c => c == random);
          if (existingColor.length == 0) {
            result.push(random);
            break;
          }
        }
      }
    }
    return result;
  }
  private generateProfitAndCostChart() {
    setTimeout(() => {
      let cols = [];
      this.profitAndCost.forEach(v => {
        cols.push([v.product, v.profit]);
      });
      const chart = c3.generate({
        bindto: '#profitAndCost',
        data: {
          json: this.profitAndCost,
          type: 'bar',
          keys: {
            value: ['profitPercentage']
          }
        },
        axis: {
          x: {
            type: 'category',
            categories: this.profitAndCost.map(v => v.product)
          }
        }
      });
    }, 1);
  }
}
