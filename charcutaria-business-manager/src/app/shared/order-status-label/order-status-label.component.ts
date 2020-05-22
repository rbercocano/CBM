import { Component, OnInit, Input } from '@angular/core';
import { OrderStatus } from '../models/orderStatus';

@Component({
  selector: 'app-order-status-label',
  templateUrl: './order-status-label.component.html',
  styleUrls: ['./order-status-label.component.scss']
})
export class OrderStatusLabelComponent implements OnInit {
  @Input("status") status: OrderStatus;
  public labelClass = '';
  constructor() { }

  ngOnInit(): void {
    switch (this.status.orderStatusId) {
      case 1: this.labelClass = 'label-info'; break;
      case 2: this.labelClass = 'label-primary'; break;
      case 3: this.labelClass = 'label-success'; break;
      case 4: this.labelClass = 'label-danger'; break;
      case 5: this.labelClass = 'label-warning'; break;
      default: this.labelClass = 'label-info'; break;
    }
  }

}
