import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'total-sales',
  templateUrl: './total-sales.component.html',
  styleUrls: ['./total-sales.component.scss']
})
export class TotalSalesComponent implements OnInit {
  @Input() totalSales: number;
  @Input() currentMonthSales: number;
  
  constructor() { }

  ngOnInit(): void {
  }

}
