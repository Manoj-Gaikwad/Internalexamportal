import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { SharedService } from '../services/shared.service';

@Injectable({
  providedIn: 'root',
})
export class LoginAuthorizationGuard implements CanActivate {

  private jwtHelper = new JwtHelperService();

  constructor(private router: Router, private roleService: SharedService) {}

  canActivate(): boolean {
    const token = localStorage.getItem('token');

    // If token exists and is not expired
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      // Example: you can check for roles here if needed
      // if (this.roleService.checkForRole('SecurityAdmin')) {
      this.router.navigate(['layout']);
      // }
      return false; // prevent access to login page if already logged in
    }

    // If no token or expired, allow access (e.g., to login page)
    return true;
  }
}
