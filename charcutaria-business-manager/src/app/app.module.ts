import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { NgxSpinnerModule } from "ngx-spinner";
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SharedModule } from './shared/shared.module';
import { AuthComponent } from './layout/auth/auth.component';
import { AdminComponent } from './layout/admin/admin.component';
import { CustomerComponent } from './layout/customer/customer.component';
import { UserSearchComponent } from './pages/customer/user/user-search/user-search.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AuthService } from './shared/services/auth/auth.service';


@NgModule({
  declarations: [
    AppComponent,
    AuthComponent,
    AdminComponent,
    CustomerComponent,
    UserSearchComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgxSpinnerModule,
    SharedModule,
    BrowserAnimationsModule
  ],
  providers: [
    AuthService,
    // { provide: APP_BASE_HREF, useValue: '/my/app' }
  ],
  bootstrap: [AppComponent],
})
export class AppModule { }
