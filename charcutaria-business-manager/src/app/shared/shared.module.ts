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
import { ConfirmModalComponent } from './confirm-modal/confirm-modal.component';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { ValidatorService } from './services/validator/validator.service';
import { EmailDirective } from './directives/email-validator/email.directive';
import { DigitOnlyDirective } from './directives/digit-only/digit-only.directive';
import { CpfDirective } from './directives/cpf-validator/cpf.directive';
import { CnpjDirective } from './directives/cnpj/cnpj.directive';
import { SecurePasswordDirective } from './directives/secure-password/secure-password.directive';
import { MobilePhoneDirective } from './directives/mobile-phone/mobile-phone.directive';
import { TransactionService } from './services/transaction/transaction.service';
import { MinValueDirective } from './directives/min-value/min-value.directive';
import { TotalSalesComponent } from './dashboard-components/total-sales/total-sales.component';
import { TotalProfitComponent } from './dashboard-components/total-profit/total-profit.component';
import { TotalPendingPaymentsComponent } from './dashboard-components/total-pending-payments/total-pending-payments.component';
import { TopFiveProfitComponent } from './dashboard-components/top-five-profit/top-five-profit.component';
import { SalesLastSixMonthsComponent } from './dashboard-components/sales-last-six-months/sales-last-six-months.component';
import { TotalOrdersComponent } from './dashboard-components/total-orders/total-orders.component';
import { TopFiveSalesComponent } from './dashboard-components/top-five-sales/top-five-sales.component';



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
    SafeHtmlPipe,
    ConfirmModalComponent,
    EmailDirective,
    DigitOnlyDirective,
    CpfDirective,
    CnpjDirective,
    SecurePasswordDirective,
    MobilePhoneDirective,
    MinValueDirective,
    TotalOrdersComponent,
    TotalSalesComponent,
    TotalProfitComponent,
    TotalPendingPaymentsComponent,
    TopFiveSalesComponent,
    TopFiveProfitComponent,
    SalesLastSixMonthsComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    NgbModule,
    HttpClientModule,
    NgxMaskModule.forRoot(),
    NgMultiSelectDropDownModule.forRoot(),
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
    OrderItemStatusLabelComponent,
    ConfirmModalComponent,
    NgMultiSelectDropDownModule,
    EmailDirective,
    DigitOnlyDirective,
    CpfDirective,
    CnpjDirective,
    SecurePasswordDirective,
    MobilePhoneDirective,
    MinValueDirective,
    TotalOrdersComponent,
    TotalSalesComponent,
    TotalProfitComponent,
    TotalPendingPaymentsComponent,
    TopFiveSalesComponent,
    TopFiveProfitComponent,
    SalesLastSixMonthsComponent
  ],
  providers: [
    UserService,
    CustomerService,
    DomainService,
    ProductService,
    CorpClientService,
    NotificationService,
    DecimalPipe,
    PaginationService,
    PricingService,
    OrderService,
    ValidatorService,
    TransactionService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpsRequestInterceptor,
      multi: true
    },
    { provide: NgbDateAdapter, useClass: BRDateAdapter },
    { provide: NgbDateParserFormatter, useClass: BRDateFormatter }
  ]
})
export class SharedModule {
  static forRoot() {
    return {
      ngModule: SharedModule,
      providers: [],
    };
  }
}
