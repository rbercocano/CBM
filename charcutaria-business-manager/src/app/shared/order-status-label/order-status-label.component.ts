import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';
import { OrderStatus } from '../models/orderStatus';

@Component({
  selector: 'app-order-status-label',
  templateUrl: './order-status-label.component.html',
  styleUrls: ['./order-status-label.component.scss']
})
export class OrderStatusLabelComponent {
  @Input("status") status: OrderStatus;
  public get labelClass() {
    switch (this.status.orderStatusId) {
      case 1: return 'label-info'; break;
      case 2: return 'label-warning'; break;
      case 3: return 'label-success'; break;
      case 4: return 'label-danger'; break;
      case 5: return 'label-primary'; break;
      default: return 'label-info'; break;
    }
  }
}
