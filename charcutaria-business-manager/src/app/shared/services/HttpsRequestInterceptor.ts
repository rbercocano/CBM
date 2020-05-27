import { Injectable, NgModule } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, } from '@angular/common/http';
import { flatMap, catchError } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AuthService } from './auth/auth.service';
import { TokenInfo } from '../models/tokenInfo';

@Injectable()
export class HttpsRequestInterceptor implements HttpInterceptor {

    constructor(private authService: AuthService) { }
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (req.url == `${environment.apiUrl}/Authentication/Login` ||
            req.url == `${environment.apiUrl}/Authentication/Token/Refresh`||
            req.url == `${environment.apiUrl}/api/v1/CorpClient/Actives`)
            return next.handle(req);
        let now = new Date();
        let tokenObj: TokenInfo = this.authService.tokenInfo ?? { expiration: now, refreshToken: null, accessToken: null, authenticated: false, created: null, message: null, userData: null };
        let expired = tokenObj.expiration <= now;
        if (expired)
            return this.authService.refreshToken().pipe(flatMap(response => {
                return this.next(response, req, next);
            }));
        return this.next(tokenObj, req, next);
    }
    private next(tokenInfo: TokenInfo, req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const dupReq = req.clone({
            headers: req.headers.set('Authorization', `Bearer ${tokenInfo.accessToken}`)
        });
        return next.handle(dupReq).pipe(catchError((err, caught) => {
            var jsonToken: TokenInfo = this.authService.tokenInfo;
            if (err.status == 401 && jsonToken != null && dupReq.url != `${environment.apiUrl}/Authentication/Login`) {
                return this.authService.refreshToken().pipe(flatMap(response => {
                    return this.next(response, req, next);
                }));
            }
            console.log(err);
            throw err;
        }));
    }
}
