import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms'; 

import { UserRoutingModule } from './user-routing.module';
import { UserRegistrationComponent } from './user-registration/user-registration.component';
import { SharedModule } from 'src/app/shared/shared.module';


@NgModule({
  declarations: [UserRegistrationComponent],
  imports: [
    CommonModule,
    UserRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule
  ]
})
export class UserModule { }
