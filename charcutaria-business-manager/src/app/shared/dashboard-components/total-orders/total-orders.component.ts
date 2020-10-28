import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core';
import { NotificationService } from '../../services/notification/notification.service';
import { OrderService } from '../../services/order/order.service';

@Component({
  selector: 'total-orders',
  templateUrl: './total-orders.component.html',
  styleUrls: ['./total-orders.component.scss']
})
export class TotalOrdersComponent implements OnInit {
  public totalOrders: number;
  public totalCompletedOrders: number;
  constructor(private orderService: OrderService,
    private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.orderService.getOrderCountSummary().subscribe(r => {
      this.totalOrders = r.totalOrders;
      this.totalCompletedOrders = r.totalCompletedOrders;
    },
      e => {
        this.notificationService.notifyHttpError(e);
      });
  }

}
