import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { CorpClient } from 'src/app/shared/models/corpClient';
import { Login } from 'src/app/shared/models/login';
import { AuthService } from 'src/app/shared/services/auth/auth.service';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { flatMap } from 'rxjs/operators';
import { of } from 'rxjs';
import { UserService } from 'src/app/shared/services/user/user.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { NotificationService } from 'src/app/shared/services/notification/notification.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  @ViewChild('f', { static: true }) form: NgForm;
  public authRes: string = "";
  public corpClients: CorpClient[] = [];
  public login: Login = { accountNumber: null, password: "", username: "" };
  constructor(
    private userService: UserService,
    private authService: AuthService, private route: Router,
    private spinner: NgxSpinnerService,
    private notificationService: NotificationService) { }

  ngOnInit(): void {
  }
  signIn(): void {
    this.authRes = "";
    if (this.form.valid) {
      this.spinner.show();
      this.authService.logIn(this.login)
        .pipe(flatMap(r => {
          if (r && r.authenticated)
            return this.userService.GetUserModules(this.authService.userData.userId)
              .pipe(flatMap(m => {
                return of(r);
              }));
          else
            return of(r);
        })).subscribe(r => {
          this.spinner.hide();
          if (r && r.authenticated)
            this.route.navigate(['/']);
          else
            this.authRes = r.message;
        }, e => {
          this.spinner.hide();
          this.notificationService.notifyHttpError(e);
        });
    }
  }
}
