import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'total-profit',
  templateUrl: './total-profit.component.html',
  styleUrls: ['./total-profit.component.scss']
})
export class TotalProfitComponent implements OnInit {
  @Input() totalProfit: number;
  @Input() currentMonthProfit: number;
  constructor() { }

  ngOnInit(): void {
  }

}
