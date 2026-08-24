import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AdminService } from 'app/layout/services/admin.service';
import { isNullOrUndefined } from 'util';
import { SharedService } from 'app/layout/services/shared.service';
import { Router } from '@angular/router';
import { AccountService } from 'app/layout/services/account.service';
import { UserDataService } from 'app/layout/services/user.service';
import { SnackBarService } from 'app/layout/services/snackbar.service';
import { MatDialog } from '@angular/material/dialog';
import { WebCamDialogComponent } from 'app/layout/dialogs/webcam/webcamdialog.component';
import { namePattern, emailPattern, phoneNumberPattern, minBirthDate } from 'app/layout/entities/globalConstants';
import { Location } from '@angular/common';

@Component({
    selector: 'cg-candidate-profile',
    templateUrl: './candidate-profile.component.html',
    styleUrls: ['./candidate-profile.component.scss'],
    providers: [AccountService],
    standalone: false
})
export class CandidateProfileComponent implements OnInit {

  candidateDetailForm: FormGroup;
  isSpinner: boolean = false;
  Id: any = null;
  isEdit: boolean = false;
  public errorMsg = [];
  candidateData: any;
  candidateUpdateData: any;
  public maxDate: Date;
  public minDate:Date = minBirthDate;

  constructor(
    private _formBuilder: FormBuilder,
    private sharedService: SharedService,
    public router: Router,
    private snackBar: SnackBarService,
    private _accountService: AccountService,
    private _userDataService: UserDataService,
    private dialog: MatDialog,
    private _location: Location) {
  }

  ngOnInit() {

    this.maxDate = new Date();

    if (!isNullOrUndefined(this.sharedService.decodeTokenToGetUserId())) {
      this.Id = this.sharedService.decodeTokenToGetUserId();
      this.GetUserDetailsById();
      this.isEdit = true;
    } else {
      this.router.navigate(['']);
    }
    this.initializeForm();

  }

  initializeForm() {
    this.candidateDetailForm = this._formBuilder.group({
      Id: [this.Id],
      email: ['', [Validators.required, Validators.maxLength(50), Validators.pattern(emailPattern)]],
      firstName: ['', [Validators.required,Validators.pattern(namePattern)]],
      lastName: ['', [Validators.required,Validators.pattern(namePattern)]],
      username: [''],
      gender: ['', [Validators.required]],
      dateOfBirth: [''],
      phonenumber: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(10),Validators.pattern(phoneNumberPattern)]],
      photo:[null]
    });
  }

  GetUserDetailsById() {
    this.showSpinner()
    this._accountService.getUserDetails(this.Id)
      .subscribe((res: any) => {
        this.candidateData = res;
        this.setFormValue();
        this.hideSpinner();
      },
        err => {
          this.errorMsg = err.error[""][0];
        }
      );
  }

  setFormValue() {
    this.candidateDetailForm.controls['email'].setValue(this.candidateData.email),
      this.candidateDetailForm.controls['username'].setValue(this.candidateData.userName),
      this.candidateDetailForm.controls['firstName'].setValue(this.candidateData.firstName),
      this.candidateDetailForm.controls['lastName'].setValue(this.candidateData.lastName),
      this.candidateDetailForm.controls['phonenumber'].setValue(this.candidateData.phoneNumber),
      this.candidateDetailForm.controls['gender'].setValue(this.candidateData.gender),
      this.candidateDetailForm.controls['dateOfBirth'].setValue(this.candidateData.dateOfBirth),
      this.candidateDetailForm.controls['photo'].setValue(this.candidateData.photo)

  }

  onSubmit() {
    this.showSpinner();
    this._accountService.updateProfile(this.candidateDetailForm.value)
      .subscribe(() => {
        this.snackBar.success('Profile Updated Successfully');
        this._userDataService.getUserDetails();
        this.router.navigate(['']);
      },
        err => {
          this.hideSpinner();
          this.snackBar.error(err.error[""][0]);
          this.errorMsg = err.error[""][0];
        }, () => {
          this.hideSpinner();
        }
      );
  }

  removePhoto(){
    this.candidateData.photo = null;
    this.candidateDetailForm.controls['photo'].setValue(null);
  }

  showSpinner() { this.isSpinner = true }

  hideSpinner() { this.isSpinner = false }

  openWebCam() {
    const dialogRef = this.dialog.open(WebCamDialogComponent, {
      width: '520px',
      panelClass: ['app-no-padding-dialog'],
      data: {
      }
    });

    dialogRef.afterClosed().subscribe(res => {
      if (res == null || res.data == null) return;
      this.candidateData.photo = res.data
      this.candidateDetailForm.controls['photo'].setValue(res.data);
    });
  }

  goBack() {
    this._location.back();
  }
}

