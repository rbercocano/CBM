import { NgModule } from '@angular/core';
import { CommonModule, DecimalPipe } from '@angular/common';
import { NavbarComponent } from './navbar/navbar.component';
import { NgbModule, NgbDateParserFormatter, NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';
import { UserService } from './services/user/user.service';
import { CustomerService } from './services/customer/customer.service';
import { DomainService } from './services/domain/domain.service';
import { ProductService } from './services/product/product.service';
import { CorpClientService } from './services/corpclient/corp-client.service';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthService } from './services/auth/auth.service';
import { RouterModule } from '@angular/router';
import { ToolbarComponent } from './toolbar/toolbar.component';
import { HttpsRequestInterceptor } from './services/HttpsRequestInterceptor';
import { ToastComponent } from './toast/toast.component';
import { NotificationService } from './services/notification/notification.service';
import { CbmCurrencyPipe } from './pipes/currency/cbm-currency.pipe';
import { ServerSidePagerComponent } from './server-side-pager/server-side-pager.component';
import { PaginationService } from './services/pagination/pagination.service';
import { NgxMaskModule } from 'ngx-mask';
import { BRDateFormatter } from './ngbDatepicker/BRDateFormatter'
import { BRDateAdapter } from './ngbDatepicker/BRDateAdapter';
import { ContactInfoComponent } from './contact-info/contact-info.component';
import { CustomerContactComponent } from './customer-contact/customer-contact.component';
import { FormsModule } from '@angular/forms';
import { CustomerDetailsComponent } from './customer-details/customer-details.component';
import { PricingService } from './services/pricing/pricing.service';
import { CbmNumberPipe } from './pipes/number/cmb-number.pipe';
import { OrderService } from './services/order/order.service';
import { OrderStatusLabelComponent } from './order-status-label/order-status-label.component';
import { PaymentStatusLabelComponent } from './payment-status-label/payment-status-label.component';
import { OrderItemStatusLabelComponent } from './order-item-status-label/order-item-status-label.component';
import { SafeHtmlPipe } from './pipes/safeHtml/safe-html.pipe';




@NgModule({
  declarations: [NavbarComponent,
    ToolbarComponent,
    ToastComponent,
    CbmCurrencyPipe,
    ServerSidePagerComponent,
    ContactInfoComponent,
    CustomerContactComponent,
    CustomerDetailsComponent,
    CbmNumberPipe,
    OrderStatusLabelComponent,
    PaymentStatusLabelComponent,
    OrderItemStatusLabelComponent,
    SafeHtmlPipe],
  imports: [
    CommonModule,
    FormsModule,
    NgbModule,
    HttpClientModule,
    NgxMaskModule.forRoot(),
    RouterModule
  ],
  exports: [
    NavbarComponent,
    ToolbarComponent,
    ToastComponent,
    NgbModule,
    NgxMaskModule,
    CbmCurrencyPipe,
    CbmNumberPipe,
    SafeHtmlPipe,
    ServerSidePagerComponent,
    ContactInfoComponent,
    CustomerContactComponent,
    OrderStatusLabelComponent,
    PaymentStatusLabelComponent,
    OrderItemStatusLabelComponent
  ],
  providers: [
    UserService,
    CustomerService,
    DomainService,
    ProductService,
    CorpClientService,
    AuthService,
    NotificationService,
    DecimalPipe,
    PaginationService,
    PricingService,
    OrderService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpsRequestInterceptor,
      multi: true
    },
    { provide: NgbDateAdapter, useClass: BRDateAdapter },
    { provide: NgbDateParserFormatter, useClass: BRDateFormatter }
  ]
})
export class SharedModule { }
