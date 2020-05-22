import { Component, OnInit, Input } from '@angular/core';
import { OrderItemStatus } from '../models/orderItemStatus';

@Component({
  selector: 'app-order-item-status-label',
  templateUrl: './order-item-status-label.component.html',
  styleUrls: ['./order-item-status-label.component.scss']
})
export class OrderItemStatusLabelComponent implements OnInit {
  @Input("status") status: OrderItemStatus;
  public labelClass = '';
  constructor() { }

  ngOnInit(): void {
    switch (this.status.orderItemStatusId) {
      case 1: this.labelClass = 'label-warning'; break;
      case 2: this.labelClass = 'label-success'; break;
      default: this.labelClass = 'label-info'; break;
    }
  }

}
