import { Component, Input, OnInit } from '@angular/core';
import { NotificationService } from '../../services/notification/notification.service';
import { OrderService } from '../../services/order/order.service';

@Component({
  selector: 'total-pending-payments',
  templateUrl: './total-pending-payments.component.html',
  styleUrls: ['./total-pending-payments.component.scss']
})
export class TotalPendingPaymentsComponent implements OnInit {
  totalPendingPayments: number;
  finishedOrdersPendingPayment: number;
  constructor(private orderService: OrderService,
    private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.orderService.getPendingPaymentsSummary().subscribe(r => {
      this.totalPendingPayments = r.totalPendingPayments;
      this.finishedOrdersPendingPayment = r.finishedOrdersPendingPayment;
    },
      e => { this.notificationService.notifyHttpError(e); });
  }

}
