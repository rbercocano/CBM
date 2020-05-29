import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RawMaterialListComponent } from './raw-material-list/raw-material-list.component';


const routes: Routes = [{
  path: '',
  component: RawMaterialListComponent
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RawMaterialRoutingModule { }
