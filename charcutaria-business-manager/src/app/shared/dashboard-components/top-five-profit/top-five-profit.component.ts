import { Component, Input, OnInit } from '@angular/core';
import { ProductCostProfit } from '../../models/productCostProfit';
import { NotificationService } from '../../services/notification/notification.service';
import { ProductService } from '../../services/product/product.service';

@Component({
  selector: 'top-five-profit',
  templateUrl: './top-five-profit.component.html',
  styleUrls: ['./top-five-profit.component.scss']
})
export class TopFiveProfitComponent implements OnInit {
  profitAndCost: ProductCostProfit[] = [];
  constructor(private productService: ProductService,
    private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.productService.getCostProfit().subscribe(r => {
      this.profitAndCost = (r ?? []).slice(0,5);
    }, e => {
      this.notificationService.notifyHttpError(e);
    });
  }

}
