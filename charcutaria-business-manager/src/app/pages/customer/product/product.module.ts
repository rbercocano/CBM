import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductRoutingModule } from './product-routing.module';
import { ProductListComponent } from './product-list/product-list.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms'; 
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ProductEditComponent } from './product-edit/product-edit.component';
import { NgxMaskModule, IConfig } from 'ngx-mask'
import { NgxCurrencyModule } from "ngx-currency";
import { DataSheetComponent } from './data-sheet/data-sheet.component';
import { DataSheetDetailsComponent } from './data-sheet-details/data-sheet-details.component';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
const maskConfig: Partial<IConfig> = {
  validation: false,
};
@NgModule({
  declarations: [ProductListComponent, ProductEditComponent, DataSheetComponent, DataSheetDetailsComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    ProductRoutingModule,
    SharedModule,
    NgbModule,
    NgxMaskModule.forRoot(maskConfig),
    NgxCurrencyModule,
    CKEditorModule
  ]
})
export class ProductModule { }
