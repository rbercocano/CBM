import { Component, OnInit, ViewEncapsulation } from '@angular/core';
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
import { SalesPerMonth } from 'src/app/shared/models/salesPerMonth';
import * as moment from 'moment';
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss',
    '../../../../../../node_modules/c3/c3.min.css'],
  encapsulation: ViewEncapsulation.None
})
export class DashboardComponent implements OnInit {
  public orderCountSummary: OrderCountSummary = { rowId: '', totalCompletedOrders: 0, totalOrders: 0 };
  public pendingPaymentsSummary: PendingPaymentsSummary = { rowId: '', totalPendingPayments: 0, finishedOrdersPendingPayment: 0 };
  public salesSummary: SalesSummary = { rowId: '', totalSales: 0, currentMonthSales: 0 };
  public profitSummary: ProfitSummary = { rowId: '', totalProfit: 0, currentMonthProfit: 0 };
  public profitAndCost: ProductCostProfit[] = [];
  public production: Production[] = [];
  public monthlySales: SalesPerMonth[] = [];
  constructor(private orderService: OrderService,
    private productService: ProductService,
    private spinner: NgxSpinnerService,
    private notificationService: NotificationService,) { }

  ngOnInit(): void {

    this.spinner.show();
    forkJoin(
      this.orderService.getOrderCountSummary(),
      this.orderService.getPendingPaymentsSummary(),
      this.orderService.getSalesSummary(),
      this.orderService.getProfitSummary(),
      this.productService.getCostProfit(),
      this.productService.getProduction(),
      this.orderService.getSalesPerMonth()
    ).subscribe(r => {
      this.orderCountSummary = r[0];
      this.pendingPaymentsSummary = r[1];
      this.salesSummary = r[2];
      this.profitSummary = r[3];
      this.profitAndCost = (r[4] ?? []).slice(0, 5);
      this.production = (r[5] ?? []).slice(0, 5);
      this.monthlySales = r[6];
      this.renderMonthlySales();
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
  private renderMonthlySales() {
    setTimeout(() => {
      let profits: Array<any> = ['Lucro'];
      let sales: Array<any> = ['Vendas'];
      let months: Array<any> = [];
      this.monthlySales.forEach(v => {
        sales.push(v.totalSales);
        profits.push(v.totalProfit);
        let date = moment(v.date);
        months.push(`${date.format('MM')}/${date.format('YYYY')}`);
      });
      var chart = c3.generate({
        bindto: '#monthlysales', size: {
          height: 240
        },
        data: {
          type: 'spline',
          columns: [
            sales,
            profits
          ]
        },
        axis: {
          x: {
            type: 'category',
            categories: months
          }
        }
      });
    }, 1);
  }
}
