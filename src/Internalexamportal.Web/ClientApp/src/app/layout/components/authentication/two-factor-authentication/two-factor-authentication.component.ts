import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';

import { CustomHttpService } from 'app/layout/services/custom-http.service';
import { SharedService } from 'app/layout/services/shared.service';
import { FuseConfigService } from '@fuse/services/config.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { fuseAnimations } from '@fuse/animations';

@Component({
    // tslint:disable-next-line:component-selector
    selector: 'two-factor-authentication',
    templateUrl: './two-factor-authentication.component.html',
    styleUrls: ['./two-factor-authentication.component.scss'],
    encapsulation: ViewEncapsulation.None,
    animations: fuseAnimations,
    standalone: false
})
export class TwoFactorAuthenticationComponent implements OnInit {

  public errorMsg = '';

  constructor(private http: CustomHttpService,
    private router: Router,
    private roleService: SharedService,
    private _fuseConfigService: FuseConfigService,
    private _formBuilder: FormBuilder
  ) {
    // Configure the layout
    this._fuseConfigService.config = {
      layout: {
        navbar: {
          hidden: true
        },
        toolbar: {
          hidden: true
        },
        footer: {
          hidden: true
        },
        sidepanel: {
          hidden: true
        }
      }
    };
  }

  twoFactorAuthForm: FormGroup;

  ngOnInit() {
    this.twoFactorAuthForm = this._formBuilder.group({
      userId: localStorage.getItem('id'),
      code: ['', [Validators.required, Validators.maxLength(6), Validators.minLength(6), Validators.pattern('^[0-9]*$')]]
    });
  }

  authenticate() {
    this.errorMsg = '';
    this.http._post('account/verifyCode', this.twoFactorAuthForm.getRawValue())
      .subscribe(
        (res: any) => {
          if (res.succeeded) {
            // Set the received token in local storage. it is used in AuthGuard class, when authenticating users.
            localStorage.setItem('token', res.tokenResult.token);
            // set username to access in the landing page.
            localStorage.setItem('username', res.userName);
            this.roleService.setRoles(res.roles);
            this.roleService.setUserType(res.userType);
            this.roleService.setProfilePicture(res.profilePicture);
            // if (this.roleService.getUserType().includes("Owner")) {
            //   this.router.navigate(['layout/service-plan/owner-plans'])
            // } else {
            //   this.router.navigate(['layout/document/list']);
            // }
          }
        },
        onerror => {
          this.errorMsg = onerror.error[""][0];
        }
      );
  }
}
