import { Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

import { FuseConfigService } from '@fuse/services/config.service';
import { fuseAnimations } from '@fuse/animations';
import { CustomHttpService } from 'app/layout/services/custom-http.service';
import { Router, ActivatedRoute } from '@angular/router';
import { SnackBarService } from 'app/layout/services/snackbar.service';
import { passwordPattern } from 'app/layout/entities/globalConstants';

@Component({
    selector: 'reset-password-2',
    templateUrl: './reset-password.component.html',
    styleUrls: ['./reset-password.component.scss'],
    encapsulation: ViewEncapsulation.None,
    animations: fuseAnimations,
    standalone: false
})
export class ResetPasswordComponent implements OnInit, OnDestroy {
    resetPasswordForm: FormGroup;

	public showPassword: boolean = false;
    public showConfirmPassword: boolean = false;
    
    // Private
    private _unsubscribeAll: Subject<void> = new Subject<void>();
    


    constructor(private route: ActivatedRoute,
        private router: Router,
        private http: CustomHttpService,
        private _fuseConfigService: FuseConfigService,
        private _formBuilder: FormBuilder,
        private snackBar: SnackBarService
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

        // Set the private defaults
        this._unsubscribeAll = new Subject();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void {
        this.resetPasswordForm = this._formBuilder.group({
            newPassword: ['', [Validators.required, Validators.pattern(passwordPattern)]],
            passwordConfirm: ['', [Validators.required, confirmPasswordValidator]],
            userId: this.route.snapshot.queryParams['UserId'],
            forgotPasswordCode: encodeURIComponent(this.route.snapshot.queryParams['ForgotPasswordCode'])
        });

        // Update the validity of the 'passwordConfirm' field
        // when the 'password' field changes
        this.resetPasswordForm.get('newPassword').valueChanges
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(() => {
                this.resetPasswordForm.get('passwordConfirm').updateValueAndValidity();
            });
    }

    resetPassword() {
        this.http._post('account/resetPassword', this.resetPasswordForm.getRawValue())
            .subscribe(
                (res: any) => {
                    if (res.isValidCredential) {
                        this.snackBar.success('Password has been successfully updated.')
                                this.router.navigate(['']);
                    } else {
                        this.snackBar.open('Your password could not be reset at the moment.');
                                this.router.navigate(['']);
                    }
                },
                (error) => {
                    this.snackBar.error(error.error[""][0]);
                            this.router.navigate(['']);
                }
            );
    }

    /**
     * On destroy
     */
    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
    }
}

/**
 * Confirm password validator
 *
 * @param {AbstractControl} control
 * @returns {ValidationErrors | null}
 */
export const confirmPasswordValidator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {

    if (!control.parent || !control) {
        return null;
    }

    const password = control.parent.get('newPassword');
    const passwordConfirm = control.parent.get('passwordConfirm');

    if (!password || !passwordConfirm) {
        return null;
    }

    if (passwordConfirm.value === '') {
        return null;
    }

    if (password.value === passwordConfirm.value) {
        return null;
    }

    return { 'passwordsNotMatching': true };
};
