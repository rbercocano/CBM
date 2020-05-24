import { Component, OnInit } from '@angular/core';
import { OrderService } from 'src/app/shared/services/order/order.service';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { NotificationService } from 'src/app/shared/services/notification/notification.service';
import { OrderDetails } from 'src/app/shared/models/orderDetails';

@Component({
  selector: 'app-order-details',
  templateUrl: './order-details.component.html',
  styleUrls: ['./order-details.component.scss']
})
export class OrderDetailsComponent implements OnInit {
  public title = 'Detalhes do Pedido';
  public order: OrderDetails;
  constructor(private orderService: OrderService,
    private route: ActivatedRoute,
    private spinner: NgxSpinnerService,
    private router: Router,
    private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.spinner.show();
    let orderNumber = this.route.snapshot.params.id;
    this.orderService.getByNumber(orderNumber).subscribe(r => {
      this.order = r;
      this.spinner.hide();
    }, e => {
      this.spinner.hide();
      this.notificationService.notifyHttpError(e);
    });
  }
  edit() {
    this.router.navigate(['/order/edit', this.order.orderNumber]);
  }
}
