import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ReportRoutingModule } from './report-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms'; 
import { SharedModule } from 'src/app/shared/shared.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxMaskModule, IConfig } from 'ngx-mask';
import { NgxCurrencyModule } from 'ngx-currency';
import { OrderItemComponent } from './order-item/order-item.component';


const maskConfig: Partial<IConfig> = {
  validation: false,
};
@NgModule({
  declarations: [OrderItemComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    ReportRoutingModule,
    SharedModule,
    NgbModule,
    NgxMaskModule.forRoot(maskConfig),
    NgxCurrencyModule,
  ]
})
export class ReportModule { }
