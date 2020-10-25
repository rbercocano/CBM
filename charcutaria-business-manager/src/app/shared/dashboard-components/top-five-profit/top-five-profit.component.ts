import { Component, Input, OnInit } from '@angular/core';
import { ProductCostProfit } from '../../models/productCostProfit';

@Component({
  selector: 'top-five-profit',
  templateUrl: './top-five-profit.component.html',
  styleUrls: ['./top-five-profit.component.scss']
})
export class TopFiveProfitComponent implements OnInit {
  @Input("data") profitAndCost: ProductCostProfit[] = [];
  constructor() { }

  ngOnInit(): void {
  }

}
