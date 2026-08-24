import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot } from '@angular/router';

import { SharedService } from '../services/shared.service';
import { Roles } from '../entities/globalConstants';

@Injectable()
export class RouteAuthorizationGuard implements CanActivate {

    constructor(private router: Router, private roleService: SharedService) {
    }

    canActivate(route: ActivatedRouteSnapshot) {
        const role = route.data.role;        

        if (localStorage.getItem("token") != null) {
            // Check for role in local storage.
            if (this.roleService.checkForRole(Roles.Candidate)) {
                // If the role is not admin, then users are not authorized to access those pages.
                if (role == this.roleService.getRole()) {
                    return true;
                }
                this.router.navigate(['/layout']);
                return false;
            } else {
                if (role == Roles.Candidate) {
                    this.router.navigate(['/layout']);
                    return false;
                }
                return true;
            }

            return true;
        } //return true;
        else {
            // If token expired , the route to login page.
            localStorage.clear();
            this.router.navigate(['']);
            return true;
        }
    }

}
