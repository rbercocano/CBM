import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProductListComponent } from './product-list/product-list.component';
import { ProductEditComponent } from './product-edit/product-edit.component';
import { DataSheetComponent } from './data-sheet/data-sheet.component';
import { DataSheetDetailsComponent } from './data-sheet-details/data-sheet-details.component';


const routes: Routes = [{
  path: '',
  component: ProductListComponent
}, {
  path: 'new',
  component: ProductEditComponent
}, {
  path: 'edit/:id',
  component: ProductEditComponent
}, {
  path: ':id/datasheet',
  component: DataSheetComponent
}, {
  path: 'datasheet/details',
  component: DataSheetDetailsComponent
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProductRoutingModule { }
