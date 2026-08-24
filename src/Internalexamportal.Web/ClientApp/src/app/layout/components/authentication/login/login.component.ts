import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FuseConfigService } from '@fuse/services/config.service';
import { fuseAnimations } from '@fuse/animations';
import { CustomHttpService } from 'app/layout/services/custom-http.service';
import { Router } from '@angular/router';
import { SharedService } from 'app/layout/services/shared.service';
import { HttpClient } from '@angular/common/http';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss'],
    encapsulation: ViewEncapsulation.None,
    animations: fuseAnimations,
    standalone: false
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup;

  /**
   * Constructor
   *
   * @param {FuseConfigService} _fuseConfigService
   * @param {FormBuilder} _formBuilder
   */
  constructor(
    private _fuseConfigService: FuseConfigService,
    private _formBuilder: FormBuilder,
    private http: CustomHttpService,
    private router: Router,
    private roleService: SharedService
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

  // -----------------------------------------------------------------------------------------------------
  // @ Lifecycle hooks
  // -----------------------------------------------------------------------------------------------------

  /**
   * On init
   */
  ngOnInit(): void {
    this.loginForm = this._formBuilder.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required]],
      rememberMe: false
    });
  }
  errorMsg: string;
  onSubmit() {
    this.errorMsg = '';
    this.http._post('account/token', this.loginForm.value)
      .subscribe(
        (res: any) => {

          localStorage.setItem('token', res.tokenResult.token);
          localStorage.setItem('id', res.userId);
          this.roleService.setRoles(res.role);
             this.roleService.setUserType(res.userType);
            this.router.navigate(['layout/admin/dashboard']);},
        err => {
          this.errorMsg = err.error[""][0];
        }
      );
  }
}
