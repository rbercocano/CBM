import { Component, OnInit, Input } from '@angular/core';
import { OrderItemStatus } from '../models/orderItemStatus';

@Component({
  selector: 'app-order-item-status-label',
  templateUrl: './order-item-status-label.component.html',
  styleUrls: ['./order-item-status-label.component.scss']
})
export class OrderItemStatusLabelComponent {
  @Input("status") status: OrderItemStatus;
  public get labelClass(): string {
    switch (this.status.orderItemStatusId) {
      case 1: return 'label-warning'; break;
      case 2: return 'label-primary'; break;
      case 3: return 'label-danger'; break;
      case 4: return 'label-info'; break;
      case 5: return 'label-success'; break;
      default: return 'label-info'; break;
    }
  }

}
