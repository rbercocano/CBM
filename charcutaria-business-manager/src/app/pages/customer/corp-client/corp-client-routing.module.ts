import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EditCorpClientComponent } from './edit-corp-client/edit-corp-client.component';


const routes: Routes = [{
  path: '',
  component: EditCorpClientComponent
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CorpClientRoutingModule { }
