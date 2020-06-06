import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AuthRoutingModule } from './auth-routing.module';
import { LoginComponent } from './login/login.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { FormsModule } from '@angular/forms';
import { ForgotComponent } from './forgot/forgot.component';


@NgModule({
  declarations: [LoginComponent, ForgotComponent],
  imports: [
    CommonModule,
    FormsModule,
    AuthRoutingModule,
    SharedModule
  ]
})
export class AuthModule { }
