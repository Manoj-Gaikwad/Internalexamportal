import { Component, OnInit, OnDestroy } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { FormGroup, FormBuilder, Validators, ValidatorFn, AbstractControl, ValidationErrors } from '@angular/forms';
import { numbersPattern, upperCasePattern, lowerCasePattern, specialPattern, passwordPattern } from 'app/layout/entities/globalConstants';
import { SharedService } from 'app/layout/services/shared.service';
import { Subscription } from 'rxjs';
import { SnackBarService } from 'app/layout/services/snackbar.service';
import { AccountService } from 'app/layout/services/account.service';

@Component({
    selector: 'app-change-password',
    templateUrl: './change-password.component.html',
    styleUrls: ['./change-password.component.scss'],
    standalone: false
})
export class ChangePasswordComponent implements OnInit, OnDestroy {

  public changePasswordForm: FormGroup;
  public isInProccess: boolean = false
  public isSpinner: boolean = false;

  showPassword: boolean = false;
  showConfirm: boolean = false;
  showNew: boolean = false;

  $subscription: Subscription[] = [];

  constructor(private _formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<any>,
    private _sharedService: SharedService,
    private snackBarService: SnackBarService,
    private _accountService: AccountService) { this.initilizeForm(); }

  ngOnInit() {
    const userId = this._sharedService.decodeTokenToGetUserId();
    this.changePasswordForm.get('userId').setValue(userId);

    this.subscribeToPasswordContronl();
  }

  subscribeToPasswordContronl() {
    let subscription = this.changePasswordForm.get('newPassword').valueChanges.subscribe(v => {
      this.changePasswordForm.get('confirmPassword').updateValueAndValidity();
    });

    this.$subscription.push(subscription);
  }

  initilizeForm() {
    this.changePasswordForm = this._formBuilder.group({
      userId: [''],
      currentPassword: ['', [Validators.required,]],
      newPassword: ['', [Validators.required, Validators.pattern(passwordPattern), Validators.minLength(8)]],
      confirmPassword: ['', [Validators.required, matchPasswords]]
    });
  }

  onChangePassword() {
    this.isInProccess = true;
    this.isSpinner = true;
    let modal = {
      currentPassword: this.changePasswordForm.get('currentPassword').value,
      newPassword: this.changePasswordForm.get('newPassword').value,
      userId: this.changePasswordForm.get('userId').value
    }

    this._accountService.changePassword(modal)
      .subscribe(() => {
        this.snackBarService.success('Password changed successfully.');
        this.close();

      }, err => {
        let erroMessage = (err.status === 400) ? err.error : 'Something went wrong.'
        this.snackBarService.error(erroMessage);
        this.close();
      }, () => {
        this.isSpinner = false;
        //this._sharedService.logout();
      }
      );
  }

  removeWhiteSpace(ev) {
    let value: string = this.changePasswordForm.get(ev).value;
    if (value.includes(' ')) {
      this.changePasswordForm.get(ev).setValue(value.replace(/ /g, ''));
      this.changePasswordForm.get(ev).updateValueAndValidity();
    }
  }

  close() {
    this.dialogRef.close();
    this.changePasswordForm.reset();
    this.isInProccess = false;
    this.isSpinner = false;
  }

  ngOnDestroy() {
    this.$subscription.forEach(v => v.unsubscribe());
  }

}

export const matchPasswords: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {

  if (!control.parent || !control) {
    return null;
  }

  const password = control.parent.get('newPassword');
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

  return { 'passworddMismatch': true };
};
