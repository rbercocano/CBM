import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProductListComponent } from './product-list/product-list.component';
import { ProductEditComponent } from './product-edit/product-edit.component';


const routes: Routes = [{
  path: '',
  component: ProductListComponent
}, {
  path: 'new',
  component: ProductEditComponent
}, {
  path: 'edit/:id',
  component: ProductEditComponent
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProductRoutingModule { }
