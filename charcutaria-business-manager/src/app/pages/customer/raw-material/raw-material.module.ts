import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RawMaterialRoutingModule } from './raw-material-routing.module';
import { RawMaterialListComponent } from './raw-material-list/raw-material-list.component';
import { IConfig, NgxMaskModule } from 'ngx-mask';
import { FormsModule, ReactiveFormsModule } from '@angular/forms'; 
import { SharedModule } from 'src/app/shared/shared.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxCurrencyModule } from 'ngx-currency';


const maskConfig: Partial<IConfig> = {
  validation: false,
};
@NgModule({
  declarations: [RawMaterialListComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RawMaterialRoutingModule,
    SharedModule,
    NgbModule,
    NgxMaskModule.forRoot(maskConfig),
    NgxCurrencyModule
 ]
})
export class RawMaterialModule { }
