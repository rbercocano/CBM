import { Component, Input, OnInit } from '@angular/core';
import { NotificationService } from '../../services/notification/notification.service';
import { OrderService } from '../../services/order/order.service';

@Component({
  selector: 'total-profit',
  templateUrl: './total-profit.component.html',
  styleUrls: ['./total-profit.component.scss']
})
export class TotalProfitComponent implements OnInit {
  totalProfit: number;
  currentMonthProfit: number;
  constructor(private orderService: OrderService,
    private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.orderService.getProfitSummary().subscribe(r => {
      this.totalProfit = r.totalProfit;
      this.currentMonthProfit = r.currentMonthProfit;
    },
      e => { this.notificationService.notifyHttpError(e); });
  }

}
