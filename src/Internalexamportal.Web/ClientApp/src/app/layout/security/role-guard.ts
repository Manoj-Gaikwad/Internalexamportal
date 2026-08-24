import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { SharedService } from '../services/shared.service';
import { PermissionEnum } from '../entities/permission.enum';

@Injectable({
  providedIn: 'root',
})
export class RoleGuard implements CanActivate {

  private jwtHelper = new JwtHelperService();
  public userRolePermissions: any[] = [];

  constructor(private router: Router, private sharedService: SharedService) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const permissionKey = route.data['permissionKey'];
    const token = this.getToken();

    // Check if token exists and is not expired
    if (!token || this.jwtHelper.isTokenExpired(token)) {
      this.router.navigate(['']);
      return false;
    }

    // Check if user has required permissions
    if (this.isAuthorized(permissionKey)) {
      return true;
    }

    // Not authorized
    this.router.navigate(['']); // Optional: navigate to unauthorized page
    return false;
  }

  private isAuthorized(permissionKey: number | number[]): boolean {
    this.userRolePermissions = this.sharedService.getUserRolePermissions();

    if (Array.isArray(permissionKey) && permissionKey.length > 1) {
      return this.userRolePermissions.some(
        prop => prop.PermissionId === permissionKey[0] || prop.PermissionId === permissionKey[1]
      );
    }

    return this.userRolePermissions.some(prop => prop.PermissionId === permissionKey);
  }

  private getToken(): string | null {
    return localStorage.getItem('token');
  }
}
