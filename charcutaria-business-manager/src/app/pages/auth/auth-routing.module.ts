import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { ForgotComponent } from './forgot/forgot.component';


const routes: Routes = [{
  path: 'login',
  component: LoginComponent
},{
  path: 'forgot',
  component: ForgotComponent
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthRoutingModule { }
