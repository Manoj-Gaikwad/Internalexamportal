import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { FuseConfigService } from '@fuse/services/config.service';
import { fuseAnimations } from '@fuse/animations';
import { PlatformLocation } from '@angular/common';
import { CustomHttpService } from 'app/layout/services/custom-http.service';
import { Router } from '@angular/router';
import { SnackBarService } from 'app/layout/services/snackbar.service';
import { emailPattern } from 'app/layout/entities/globalConstants';

@Component({
    selector: 'forgot-password-2',
    templateUrl: './forgot-password.component.html',
    styleUrls: ['./forgot-password.component.scss'],
    encapsulation: ViewEncapsulation.None,
    animations: fuseAnimations,
    standalone: false
})
export class ForgotPasswordComponent implements OnInit {
    forgotPasswordForm: FormGroup;
    public angularBaseUrl: string;

    /**
     * Constructor
     *
     * @param {FuseConfigService} _fuseConfigService
     * @param {FormBuilder} _formBuilder
     */
    constructor(
        private _fuseConfigService: FuseConfigService,
        private _formBuilder: FormBuilder,
        private platformLocation: PlatformLocation,
        private http: CustomHttpService,
        private snackBar: SnackBarService,
        private router: Router
    ) {

        this.angularBaseUrl = document.baseURI;

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
        this.forgotPasswordForm = this._formBuilder.group({
            email: ['', [Validators.required, Validators.pattern(emailPattern)]],
            baseUrl: this.angularBaseUrl + '#/reset-password?UserId={0}&ForgotPasswordCode={1}'
        });
    }

    sendLink() {
        this.http._post('account/forgotPassword', this.forgotPasswordForm.getRawValue())
            .subscribe(
                (res: any) => { 
                    if (res.succeeded) {
                        this.snackBar.success('We have sent you an email with the password reset link. Please click on the link to reset your password.');
                        // .afterDismissed().subscribe(prop => {
                        //     this.router.navigate(['']);
                        // })
                    } else {
                        this.snackBar.error('Provided email is not present in the system !')
                    }
                },
                (error) => {
                    this.snackBar.error(error.json().description)
                }
            );
    }
}
