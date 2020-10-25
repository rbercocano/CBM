import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { NotificationService } from 'src/app/shared/services/notification/notification.service';
import { UserService } from 'src/app/shared/services/user/user.service';
import { ChangePassword } from 'src/app/shared/models/ChangePassword';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent implements OnInit {
  @ViewChild('f', { static: true }) form: NgForm;
  public loading = true;
  public email: string = "";
  public login: ChangePassword = {} as ChangePassword;
  constructor(private notificationService: NotificationService,
    private spinner: NgxSpinnerService,
    private userService: UserService) { }

  ngOnInit(): void {
  }
  confirm(): void {
    if (this.login.newPassword != this.login.newPasswordConfirmation) {
      this.notificationService.showError('Atenção', 'As novas senhas não coincidem');
      return;
    }
    let regexp = new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,})");
    if (!regexp.test(this.login.newPassword)) {
      this.notificationService.showError('Atenção', 'A senha não atende aos requisitos mínimos de segurança');
      return;
    }
    if (this.form.valid) {
      this.spinner.show();
      this.userService.changePassword(this.login).subscribe(r => {
        this.email = r.email;
        this.login = {} as ChangePassword;
        this.notificationService.showSuccess('Sucesso', 'Senha atualizada com sucesso');
        this.spinner.hide();

      }, e => {
        this.spinner.hide();
        this.notificationService.notifyHttpError(e);
      });
    }
  }

}
