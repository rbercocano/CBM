import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { CorpClient } from 'src/app/shared/models/corpClient';
import { CorpClientService } from 'src/app/shared/services/corpclient/corp-client.service';
import { UserService } from 'src/app/shared/services/user/user.service';
import { NotificationService } from 'src/app/shared/services/notification/notification.service';

@Component({
  selector: 'app-forgot',
  templateUrl: './forgot.component.html',
  styleUrls: ['./forgot.component.scss']
})
export class ForgotComponent implements OnInit {
  @ViewChild('f', { static: true }) form: NgForm;
  public email: string = "";
  public login = { accountNumber: "", username: "" };
  constructor(private notificationService: NotificationService,
    private userService: UserService) { }

  ngOnInit(): void {
  }
  reset(): void {
    this.email = "";
    if (this.form.valid) {
      this.userService.resetPassword(this.login).subscribe(r => {
        this.email = r.email;
      }, e => {
        this.notificationService.notifyHttpError(e);
      });
    }
  }

}
