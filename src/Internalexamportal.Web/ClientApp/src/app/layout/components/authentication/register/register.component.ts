import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { PlatformLocation } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ValidatorFn, AbstractControl, ValidationErrors, FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { CustomHttpService } from 'app/layout/services/custom-http.service';
import { FuseConfigService } from '@fuse/services/config.service';
import { fuseAnimations } from '@fuse/animations';
import { TermsAndConditionsComponent } from '../terms-and-conditions/terms-and-conditions.component';
import { AccountService } from 'app/layout/services/account.service';
import { Subscription } from 'rxjs';
import { SnackBarService } from 'app/layout/services/snackbar.service';
import { passwordPattern, phoneNumberPattern, inputNamePattern, emailPattern } from 'app/layout/entities/globalConstants';

@Component({
    // tslint:disable-next-line:component-selector
    selector: 'register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.scss'],
    encapsulation: ViewEncapsulation.None,
    animations: fuseAnimations,
    standalone: false
})
export class RegisterComponent implements OnInit {

	public registerForm: FormGroup;
	public angularBaseUrl = '';
	public errorMsg = [];
	public email: string;
	userName: any;
	subscriptions: Subscription[] = [];
	public showPassword: boolean = false;
	public showConfirmPassword: boolean = false;

	constructor(private activatedRoute: ActivatedRoute, private router: Router,
		private platformLocation: PlatformLocation, private accountService: AccountService,
		private snackBar: SnackBarService,
		private _fuseConfigService: FuseConfigService,
		public dialog: MatDialog,
		private _formBuilder: FormBuilder
	) {
		this.angularBaseUrl = window.location.origin;

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
	ngOnInit() {
		localStorage.clear();
		this.registerForm = this._formBuilder.group({
			firstName: ['', [Validators.required, Validators.maxLength(30), Validators.pattern(inputNamePattern)]],
			lastName: ['', [Validators.required, Validators.maxLength(30), Validators.pattern(inputNamePattern)]],
			userName: [''],
			phoneNumber: ['', [Validators.required, Validators.maxLength(10), Validators.minLength(10), Validators.pattern(phoneNumberPattern)]],
			password: ['', [Validators.required, Validators.pattern(passwordPattern)]],
			confirmPassword: ['', [Validators.required, confirmPasswordValidator]],
			email: ['', [Validators.required, Validators.maxLength(50), Validators.pattern(emailPattern)], this.isUserNameUnique.bind(this)],
			agreedToTerms: true
		});

		this.registerForm.controls.password.valueChanges.subscribe(value => {
			this.registerForm.controls.confirmPassword.updateValueAndValidity();
		})
	}

	showTermsAndCondition() {
		const dialogRef = this.dialog.open(TermsAndConditionsComponent);
		dialogRef.afterClosed().subscribe(result => {
			if (result) {
				this.registerForm.controls['agreedToTerms'].setValue(true);
			} else {
				this.registerForm.controls['agreedToTerms'].setValue(false);
			}
		});
	}

	isUserNameUnique(control: FormControl) {
		const q = new Promise((resolve, reject) => {
			setTimeout(() => {
				this.unsubscribeSubscriptions();
				let subscription = this.accountService.checkifUserExists({ UserName: control.value })
					.subscribe(res => {
						if (res.userNameExists) {
							resolve({ 'isUserNameUnique': true });
						} else {
							resolve(null);
						}
					});
				this.subscriptions.push(subscription);
			}, 0);
		});
		return q;
	}

	register() {
		this.errorMsg = [];
		var formValue = this.registerForm.value;
		formValue.userName = formValue.email;
		if (this.registerForm.valid) {
			this.accountService.register(formValue)
				.subscribe(res => {
					this.snackBar.success('Candidate Registered Successfully');
					this.router.navigate(['./']);
				}, err => {
					this.snackBar.error(err.error['']);
				}, () => {

				});
		} else {
			console.log(this.registerForm);
		}
	}

	unsubscribeSubscriptions() {
		if (this.subscriptions && this.subscriptions.length > 0) {
			this.subscriptions.forEach(subscription => { subscription.unsubscribe() });
		}
	}

	ngOnDestroy() {
		this.unsubscribeSubscriptions();
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

	const password = control.parent.get('password');
	const passwordConfirm = control.parent.get('confirmPassword');

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

