import { ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { CorpClient } from 'src/app/shared/models/corpClient';
import { CustomerType } from 'src/app/shared/models/customerType';
import { AuthService } from 'src/app/shared/services/auth/auth.service';
import { CorpClientService } from 'src/app/shared/services/corpclient/corp-client.service';
import { NotificationService } from 'src/app/shared/services/notification/notification.service';

@Component({
  selector: 'app-edit-corp-client',
  templateUrl: './edit-corp-client.component.html',
  styleUrls: ['./edit-corp-client.component.scss']
})
export class EditCorpClientComponent implements OnInit {
  @ViewChild('f', { static: true }) form: NgForm;
  public customerTypes: CustomerType[] = [{ description: 'Pessoa Física', customerTypeId: 1 }, { description: 'Pessoa Jurídica', customerTypeId: 2 }];
  public client: CorpClient = {} as CorpClient;
  public currencies: string[] = ['R$', '$'];
  public editing = false;
  public currentClient: CorpClient = {} as CorpClient;
  constructor(private corpClientService: CorpClientService,
    private authService: AuthService,
    private spinner: NgxSpinnerService,
    private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.spinner.show();
    this.corpClientService.GetCorpClient(this.authService.userData.corpClientId).subscribe(r => {
      this.client = r;
      this.currentClient = { ...r };
      this.spinner.hide();
      this.editing = false;
    }, (e) => {
      this.spinner.hide();
      this.notificationService.notifyHttpError(e);
    });
  }
  public save(): void {
    if (this.form.invalid) return;
    this.spinner.show();
    this.corpClientService.Update(this.client).subscribe(r => {
      this.spinner.hide();
      this.editing = false;
      this.notificationService.showSuccess('Sucesso', 'Dados atualizados com sucesso');
    }, (e) => {
      this.spinner.hide();
      this.notificationService.notifyHttpError(e);
    });
  }
  cancel(): void {
    this.client = { ...this.currentClient };

  }
}
