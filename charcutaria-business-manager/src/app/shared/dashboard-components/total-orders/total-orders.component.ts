import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'total-orders',
  templateUrl: './total-orders.component.html',
  styleUrls: ['./total-orders.component.scss']
})
export class TotalOrdersComponent implements OnInit {
  @Input() totalOrders: number;
  @Input() totalCompletedOrders: number;
  constructor() { }

  ngOnInit(): void {
  }

}
