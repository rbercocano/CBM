import { Component, Input, OnInit } from '@angular/core';
import { Production } from '../../models/production';
import { NotificationService } from '../../services/notification/notification.service';
import { ProductService } from '../../services/product/product.service';

@Component({
  selector: 'top-five-sales',
  templateUrl: './top-five-sales.component.html',
  styleUrls: ['./top-five-sales.component.scss']
})
export class TopFiveSalesComponent implements OnInit {
  production: Production[] = [];
  constructor(private productService: ProductService,
    private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.productService.getProduction().subscribe(r => {
      this.production = (r ?? []).slice(0,5);
    }, e => {
      this.notificationService.notifyHttpError(e);
    });
  }

}
