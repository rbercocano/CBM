import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'total-pending-payments',
  templateUrl: './total-pending-payments.component.html',
  styleUrls: ['./total-pending-payments.component.scss']
})
export class TotalPendingPaymentsComponent implements OnInit {
  @Input() totalPendingPayments: number;
  @Input() finishedOrdersPendingPayment: number;
  constructor() { }

  ngOnInit(): void {
  }

}
