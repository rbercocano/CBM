import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { CorpClientService } from 'src/app/shared/services/corpclient/corp-client.service';
import { CorpClient } from 'src/app/shared/models/corpClient';
import { Login } from 'src/app/shared/models/login';
import { AuthService } from 'src/app/shared/services/auth/auth.service';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  @ViewChild('f', { static: true }) form: NgForm;
  public authRes: string = "";
  public corpClients: CorpClient[] = [];
  public login: Login = { corpClientId: null, password: "", username: "" };
  constructor(private corpClientService: CorpClientService, private authService: AuthService, private route: Router) { }

  ngOnInit(): void {
    this.corpClientService.GetActives().subscribe(c => this.corpClients = c);
  }
  signIn(): void {
    this.authRes = "";
    if (this.form.valid) {
      this.authService.logIn(this.login)
        .subscribe(r => {
          if (r && r.authenticated)
            this.route.navigate(['/']);
          else
            this.authRes = r.message;
        });
    }
  }
}
