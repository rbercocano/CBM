import { Component, OnInit, Input } from '@angular/core';
import { PaymentStatus } from '../models/paymentStatus';

@Component({
  selector: 'app-payment-status-label',
  templateUrl: './payment-status-label.component.html',
  styleUrls: ['./payment-status-label.component.scss']
})
export class PaymentStatusLabelComponent {
  @Input("status") status: PaymentStatus;
  public get labelClass(): string {
    switch (this.status.paymentStatusId) {
      case 1: return 'label-warning'; break;
      case 2: return 'label-success'; break;
      default: return 'label-info'; break;
    }
  }
}
