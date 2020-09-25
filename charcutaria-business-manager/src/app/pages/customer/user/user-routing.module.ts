import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UserSearchComponent } from './user-search/user-search.component';
import { UserRegistrationComponent } from './user-registration/user-registration.component';
import { ChangePasswordComponent } from './change-password/change-password.component';


const routes: Routes = [{
  path: '',
  component: UserSearchComponent
},
{
  path: 'create',
  component: UserRegistrationComponent
},
{
  path: 'edit',
  component: UserRegistrationComponent
},
{
  path: 'change-password',
  component: ChangePasswordComponent
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }
