import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UserSearchComponent } from './user-search/user-search.component';
import { UserRegistrationComponent } from './user-registration/user-registration.component';


const routes: Routes = [{
  path: '',
  component: UserSearchComponent,
  data: {
    title: 'Search'
  }
},
{
  path: 'create',
  component: UserRegistrationComponent,
  data: {
    title: 'Create'
  }
},
{
  path: 'edit',
  component: UserRegistrationComponent,
  data: {
    title: 'Edit'
  }
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }
