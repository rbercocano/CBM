import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { OrderCountSummary } from 'src/app/shared/models/orderCountSummary';
import { PendingPaymentsSummary } from 'src/app/shared/models/pendingPaymentSummary';
import { SalesSummary } from 'src/app/shared/models/salesSummary';
import { ProfitSummary } from 'src/app/shared/models/profitSummary';
import { OrderService } from 'src/app/shared/services/order/order.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { NotificationService } from 'src/app/shared/services/notification/notification.service';
import { forkJoin } from 'rxjs';
import { ProductService } from 'src/app/shared/services/product/product.service';
import { ProductCostProfit } from 'src/app/shared/models/productCostProfit';
import { Production } from 'src/app/shared/models/production';
import { SalesPerMonth } from 'src/app/shared/models/salesPerMonth';
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
    }, e => {
      this.notificationService.notifyHttpError(e);
      this.spinner.hide();
    }, () => {
      this.spinner.hide();
    })
  }
}
