import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import * as CryptoJS from 'crypto-js';
import { FuseNavigationService } from '@fuse/components/navigation/navigation.service';
import { PermissionEnum } from '../entities/permission.enum';
import { Roles, cryptoKey } from '../entities/globalConstants';
import { MatPaginator } from '@angular/material/paginator';

@Injectable({
  providedIn: 'root',
})
export class SharedService {
  private static profilePicture: string;
  jwtHelper = new JwtHelperService();
  public userRolePermissions: any[] = [];

  constructor(private _fuseNavigationService: FuseNavigationService) { }

  // Profile picture management
  setProfilePicture(profilePicture: string) {
    SharedService.profilePicture = profilePicture;
  }

  getProfilePicture(): string {
    return SharedService.profilePicture;
  }

  // Roles & token
  setRoles(role: string[]) {
    localStorage.setItem('role', JSON.stringify(role));
  }

  setToken(token: string) {
    localStorage.setItem('token', token);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  setUserType(userType: string) {
    localStorage.setItem('userType', userType);
  }

  getUserType(): string | null {
    return localStorage.getItem('userType');
  }

  clearRole() {
    localStorage.removeItem('role');
  }

  decodeToken(): any | null {
    const token = this.getToken();
    return token ? this.jwtHelper.decodeToken(token) : null;
  }

  getRole(): string | null {
    const jwtuser = this.decodeToken();
    if (jwtuser) {
      return jwtuser['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || null;
    }
    return null;
  }

  checkForRole(role: string): boolean {
    return this.getRole() === role;
  }

  decodeTokenToGetUserId(): string | null {
    const jwtuser = this.decodeToken();
    return jwtuser ? jwtuser.sid : null;
  }

  getUserFromToken(): { name: string; email: string } | null {
    const jwtuser = this.decodeToken();
    if (!jwtuser) return null;

    return {
      name: jwtuser['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'],
      email: jwtuser['email'],
    };
  }

  setUserRolePermissions(permissions) {
    var encodedPermissions = CryptoJS.AES
      .encrypt(
        permissions.toString().trim(),
        cryptoKey.trim())
      .toString();
    localStorage.setItem('permissions', encodedPermissions);
  }


getUserRolePermissions() {
  const encryptedPermissions = localStorage.getItem("permissions");
  
  if (!encryptedPermissions) {
    console.warn("No permissions found in localStorage");
    return null; // or return {} if you prefer
  }

  if (!cryptoKey) {
    console.error("Crypto key is not defined");
    return null;
  }

  const decodedPermissions = CryptoJS.AES
    .decrypt(encryptedPermissions.trim(), cryptoKey.trim())
    .toString(CryptoJS.enc.Utf8);

  try {
    return JSON.parse(decodedPermissions);
  } catch (e) {
    console.error("Failed to parse permissions JSON", e);
    return null;
  }
}

 

  // Navigation
isAuthorizedNavigationItems() {
    const permissions = this.getUserRolePermissions() || []; // default to empty array

    if (permissions.length === 0) return;

    this.userRolePermissions = permissions;

    this.navigationPermission(PermissionEnum.ViewDashboard, 'dashboard');
    this.navigationPermission(PermissionEnum.ViewQuestion, 'question-grid');
    this.navigationPermission(PermissionEnum.ViewSubject, 'subject');
    this.navigationPermission(PermissionEnum.ViewTest, 'test');
    this.navigationPermission(PermissionEnum.ViewCandidate, 'candidate');
    this.navigationPermission(PermissionEnum.ViewAdmin, 'admin');
    this.navigationPermission(PermissionEnum.RolePermissions, 'role-permission');

    this._fuseNavigationService.updateNavigationItem('client', {
        hidden: !this.checkForRole(Roles.SuperAdmin),
    });
}


  private navigationPermission(permissionEnum: PermissionEnum, navItem: string) {
    const hidden = this.userRolePermissions.findIndex(
      (prop) => prop.PermissionId === permissionEnum
    ) < 0;
    this._fuseNavigationService.updateNavigationItem(navItem, { hidden });
  }

  // Table row index helper
  getRowIndex(i: number, paginator: MatPaginator): number {
    return i + 1 + (paginator.pageIndex * paginator.pageSize);
  }
}
