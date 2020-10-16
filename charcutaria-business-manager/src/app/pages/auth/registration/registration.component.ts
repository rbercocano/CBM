import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { ClientRegistration } from 'src/app/shared/models/clientRegistration';
import { CorpClient } from 'src/app/shared/models/corpClient';
import { CustomerType } from 'src/app/shared/models/customerType';
import { CorpClientService } from 'src/app/shared/services/corpclient/corp-client.service';
import { NotificationService } from 'src/app/shared/services/notification/notification.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {
  @ViewChild('f', { static: true }) form: NgForm;
  public registered:boolean=false;
  public client: ClientRegistration = {} as ClientRegistration;
  public corpClient: CorpClient = {} as CorpClient;
  public customerTypes: CustomerType[] = [{ description: 'Pessoa Física', customerTypeId: 1 }, { description: 'Pessoa Jurídica', customerTypeId: 2 }];
  constructor(private corpClientService: CorpClientService,
    private spinner: NgxSpinnerService,
    private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.client.customerTypeId = 2;
  }
  public signUp(): void {
    if (this.form.invalid) return;
    this.spinner.show();
    this.corpClientService.Register(this.client).subscribe(r => {
      this.corpClient = r;
      this.registered = true;
      this.spinner.hide();
    }, (e) => {
      this.spinner.hide();
      this.notificationService.notifyHttpError(e);
    });
  }
}
