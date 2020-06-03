import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { AuthService } from '../services/auth/auth.service';
import { Injectable } from '@angular/core';
import { UserService } from '../services/user/user.service';

@Injectable({
    providedIn: 'root'
})
export class AuthGuard implements CanActivate {
    constructor(private authService: AuthService, private router: Router, private userService: UserService) { }

    canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
      
        if (!this.authService.isAuthenticated) {
            this.router.navigate(['/auth/login'], { queryParams: { returnUrl: state.url } });
            return false;
        } else {
            return true;
            if (this.hasAccess(state.url))
                return true;
            else {
                this.router.navigate(['/dashboard']);
                return false;
            }
        }
    }
    private hasAccess(url: string): boolean {
        let modules = this.userService.userModules;
        for (let i = 0; i < modules.length; i++) {
            var parentRoute = modules[i];
            if (parentRoute.route == url)
                return true;
            else {
                for (let c = 0; c < parentRoute.childModules.length; c++) {
                    var childRoute = parentRoute.childModules[c];
                    if (childRoute.route == url)
                        return true;
                }
            }
        }
        return false;
    }
}