import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderRoutingModule } from './order-routing.module';
import { NewOrderComponent } from './new-order/new-order.component';
import { IConfig, NgxMaskModule } from 'ngx-mask';
import { FormsModule } from '@angular/forms';
import { SharedModule } from 'src/app/shared/shared.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxCurrencyModule } from 'ngx-currency';
import { OrderDetailsComponent } from './order-details/order-details.component';
import { EditOrderComponent } from './edit-order/edit-order.component';
import { OrderSearchComponent } from './order-search/order-search.component';

const maskConfig: Partial<IConfig> = {
  validation: false,
};

@NgModule({
  declarations: [NewOrderComponent, OrderDetailsComponent, EditOrderComponent, OrderSearchComponent],
  imports: [
    CommonModule,
    OrderRoutingModule,
    FormsModule,
    SharedModule,
    NgbModule,
    NgxMaskModule.forRoot(maskConfig),
    NgxCurrencyModule
  ]
})
export class OrderModule { }
