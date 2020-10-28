import { Component, Input, OnInit } from '@angular/core';
import { NotificationService } from '../../services/notification/notification.service';
import { OrderService } from '../../services/order/order.service';

@Component({
  selector: 'total-sales',
  templateUrl: './total-sales.component.html',
  styleUrls: ['./total-sales.component.scss']
})
export class TotalSalesComponent implements OnInit {
  totalSales: number;
  currentMonthSales: number;

  constructor(private orderService: OrderService,
    private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.orderService.getSalesSummary().subscribe(r => {
      this.totalSales = r.totalSales;
      this.currentMonthSales = r.currentMonthSales;
    },
      e => { this.notificationService.notifyHttpError(e); });
  }

}
