import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Login } from '../../models/login';
import { map } from 'rxjs/operators';
import { TokenInfo } from '../../models/tokenInfo';
import { JWTUserInfo } from '../../models/jwtUserInfo';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private currentUserSubject: BehaviorSubject<TokenInfo>;
  public currentUser: Observable<TokenInfo>;
  constructor(private http: HttpClient) {
    this.currentUserSubject = new BehaviorSubject<TokenInfo>(JSON.parse(localStorage.getItem('currentUser')));
    this.currentUser = this.currentUserSubject.asObservable();
  }
  public get tokenInfo(): TokenInfo {
    return this.currentUserSubject.value;
  }
  public get userData(): JWTUserInfo {
    return this.currentUserSubject.value.userData;
  }
  refreshToken(): Observable<TokenInfo> {
    const url = `${environment.apiUrl}/Authentication/Token/Refresh`;
    let data = {
      Token: this.currentUserSubject.value.accessToken,
      RefreshToken: this.currentUserSubject.value.refreshToken
    };
    let userData = this.currentUserSubject.value.userData;
    return this.http.post<TokenInfo>(url, data).pipe(map(jwt => {
      jwt.userData = userData
      localStorage.setItem('currentUser', JSON.stringify(jwt));
      this.currentUserSubject.next(jwt);
      return jwt;
    }));
  }
  logIn(loginInfo: Login): Observable<TokenInfo> {
    const url = `${environment.apiUrl}/Authentication/login`;
    return this.http.post<TokenInfo>(url, {
      clientSecret: environment.clientSecret,
      username: loginInfo.username,
      password: loginInfo.password,
      corpClientId: loginInfo.corpClientId
    }).pipe(map(jwt => {
      if (jwt && jwt.authenticated) {
        localStorage.setItem('currentUser', JSON.stringify(jwt));
        this.currentUserSubject.next(jwt);
      }
      return jwt;
    }));
  }
}
