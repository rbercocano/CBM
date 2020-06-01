import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductRoutingModule } from './product-routing.module';
import { ProductListComponent } from './product-list/product-list.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ProductEditComponent } from './product-edit/product-edit.component';
import { NgxMaskModule, IConfig } from 'ngx-mask'
import { NgxCurrencyModule } from "ngx-currency";
import { DataSheetComponent } from './data-sheet/data-sheet.component';
import { FroalaEditorModule, FroalaViewModule } from 'angular-froala-wysiwyg';
import 'froala-editor/js/plugins.pkgd.min.js';
 
import 'froala-editor/js/plugins/align.min.js';
 
import 'froala-editor/js/languages/de.js';
 
import 'froala-editor/js/third_party/font_awesome.min';
import 'froala-editor/js/third_party/image_tui.min';
import 'froala-editor/js/third_party/spell_checker.min';
import 'froala-editor/js/third_party/embedly.min';
import { DataSheetDetailsComponent } from './data-sheet-details/data-sheet-details.component';

const maskConfig: Partial<IConfig> = {
  validation: false,
};
@NgModule({
  declarations: [ProductListComponent, ProductEditComponent, DataSheetComponent, DataSheetDetailsComponent],
  imports: [
    CommonModule,
    FormsModule,
    ProductRoutingModule,
    SharedModule,
    NgbModule,
    NgxMaskModule.forRoot(maskConfig),
    NgxCurrencyModule,
    FroalaEditorModule.forRoot(),
    FroalaViewModule.forRoot()
  ]
})
export class ProductModule { }
