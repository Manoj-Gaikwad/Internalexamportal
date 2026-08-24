import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FuseConfigService } from '@fuse/services/config.service';
import { Router, ActivatedRoute } from '@angular/router';
import { MatDialog, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AdminService } from 'app/layout/services/admin.service';
import { isNullOrUndefined } from 'util';
import { inputNamePattern, emailPattern, phoneNumberPattern, genderPattern, minBirthDate } from 'app/layout/entities/globalConstants';
import { SnackBarService } from 'app/layout/services/snackbar.service';
import { CandidateModel } from 'app/layout/entities/models';
import  moment from 'moment';
import { isEmpty } from 'lodash';

@Component({
    selector: 'cg-edit-import-candidate',
    templateUrl: './edit-import-candidate.component.html',
    styleUrls: ['./edit-import-candidate.component.scss'],
    standalone: false
})
export class EditImportCandidateComponent implements OnInit {

  public title: string = "Edit Candidate Details";
  candidateDetailForm: FormGroup;
  isSpinner: boolean = false;
  isEdit: boolean = false;
  public errorMsg = [];
  public maxDate: Date;
  public minDate:Date = minBirthDate;

  public groups: Array<any>;

  constructor(@Inject(MAT_DIALOG_DATA) public candidate: CandidateModel,
    private dialogRef: MatDialogRef<any>,
    private _formBuilder: FormBuilder,
    private router: Router,
    public dialog: MatDialog,
    private snackBar: SnackBarService) {
  }

  ngOnInit() {

    this.maxDate = new Date();
    if (!isNullOrUndefined(this.candidate)) {
      this.initializeForm();
      this.setFormValue();
      this.isEdit = true;
    } else {
      this.initializeForm()
    }

  }

  initializeForm() {
    var date:moment.Moment = moment(this.candidate.DateOfBirth, 'DD/MM/YYYY');
    this.candidateDetailForm = this._formBuilder.group({
      Email: [this.candidate.Email, [Validators.required, Validators.maxLength(50), Validators.pattern(emailPattern)]],
      FirstName: [this.candidate.FirstName, [Validators.required, Validators.pattern(inputNamePattern), Validators.maxLength(20)]],
      LastName: [this.candidate.LastName, [Validators.required, Validators.pattern(inputNamePattern), Validators.maxLength(20)]],
      Gender: [this.candidate.Gender, [Validators.required,Validators.pattern(genderPattern)]],
      DateOfBirth: [date.isValid() ? date : null],
      Phone: [this.candidate.Phone, [Validators.required, Validators.pattern(phoneNumberPattern), Validators.minLength(10), Validators.maxLength(10)]],
    });
  }


  setFormValue() {
    this.candidateDetailForm.markAsTouched();
    this.markFormGroupTouched(this.candidateDetailForm)
  }

  private markFormGroupTouched(formGroup: FormGroup) {
    (<any>Object).values(formGroup.controls).forEach(control => {
      control.markAsTouched();

      if (control.controls) {
        this.markFormGroupTouched(control);
      }
    });
  }

  onSubmit() {
    if (this.isEdit) {
      this.updateCandidate();
    } else {
      this.AddCandidate();
    }
  }

  updateCandidate() {
    var candidate: CandidateModel = this.candidateDetailForm.value
    if (candidate.DateOfBirth)
      candidate.DateOfBirth = this.formatDate(candidate.DateOfBirth);
    candidate.isValid = this.candidateDetailForm.valid;
    this.dialogRef.close(candidate);
  }

  AddCandidate() {
    var candidate: CandidateModel = this.candidateDetailForm.value
    if (this.candidate.DateOfBirth)
      candidate.DateOfBirth = this.formatDate(candidate.DateOfBirth);
    candidate.isValid = this.candidateDetailForm.valid;
    this.dialogRef.close(candidate);
  }

  formatDate(date) {
    var d = new Date(date),
      month = '' + (d.getMonth() + 1),
      day = '' + d.getDate(),
      year = d.getFullYear();

    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;

    return [day, month, year].join('/');
  }

  showSpinner() { this.isSpinner = true }

  hideSpinner() { this.isSpinner = false }
}
