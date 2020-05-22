import { Component, OnInit, Input } from '@angular/core';
import { PaymentStatus } from '../models/paymentStatus';

@Component({
  selector: 'app-payment-status-label',
  templateUrl: './payment-status-label.component.html',
  styleUrls: ['./payment-status-label.component.scss']
})
export class PaymentStatusLabelComponent implements OnInit {
  @Input("status") status: PaymentStatus;
  public labelClass = '';
  constructor() { }

  ngOnInit(): void {
    switch (this.status.paymentStatusId) {
      case 1: this.labelClass = 'label-warning'; break;
      case 2: this.labelClass = 'label-success'; break;
      default: this.labelClass = 'label-info'; break;
    }
  }

}
