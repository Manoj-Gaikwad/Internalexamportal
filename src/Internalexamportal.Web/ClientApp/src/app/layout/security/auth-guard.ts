import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { RedirectService } from '../services/redirect-url.service';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationGuard implements CanActivate {

  private jwtHelper = new JwtHelperService();

  constructor(private router: Router, private redirectService: RedirectService) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const url: string = state.url;
    const token = this.getToken();

    if (token && !this.jwtHelper.isTokenExpired(token)) {
      // Handle any stored redirect URL
      if (this.redirectService.redirectUrl) {
        this.router.navigate([this.redirectService.redirectUrl]);
        this.redirectService.redirectUrl = null;
        return true;
      }
      return true;
    } else {
      // Token invalid or missing
      this.redirectService.redirectUrl = url;

      if (url === '/layout/candidates/activation') {
        this.router.navigate(['register']);
        return false;
      }

      localStorage.clear();
      this.router.navigate(['']);
      return false;
    }
  }

  public getToken(): string | null {
    return localStorage.getItem('token');
  }
}
