import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FinancesRoutingModule } from './finances-routing.module';
import { BalanceComponent } from './balance/balance.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { SharedModule } from 'src/app/shared/shared.module';
import { IConfig, NgxMaskModule } from 'ngx-mask';
import { NgxCurrencyModule } from 'ngx-currency';

const maskConfig: Partial<IConfig> = {
  validation: false,
};

@NgModule({
  declarations: [BalanceComponent],
  imports: [
    FinancesRoutingModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule,
    NgbModule,
    NgxMaskModule.forRoot(maskConfig),
    NgxCurrencyModule
  ]
})
export class FinancesModule { }
