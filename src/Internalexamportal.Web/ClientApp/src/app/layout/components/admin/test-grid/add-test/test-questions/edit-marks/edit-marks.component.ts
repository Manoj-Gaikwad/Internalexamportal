import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { isNullOrUndefined } from 'util';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';
import { CandidateModel, TestQuestion } from 'app/layout/entities/models';
import { Router } from '@angular/router';
import { SnackBarService } from 'app/layout/services/snackbar.service';
import { emailPattern, inputNamePattern, genderPattern, phoneNumberPattern, numberPattern } from 'app/layout/entities/globalConstants';
import { TestService } from 'app/layout/services/test.service';

@Component({
    selector: 'cg-edit-marks',
    templateUrl: './edit-marks.component.html',
    styleUrls: ['./edit-marks.component.scss'],
    providers: [TestService],
    standalone: false
})
export class EditMarksComponent implements OnInit {

  editMarkForm: FormGroup;
  isSpinner: boolean = false;
  isEdit: boolean = false;
  public errorMsg = [];
  public maxDate: Date;
  public testQuestion: TestQuestion;

  public groups: Array<any>;

  constructor(@Inject(MAT_DIALOG_DATA) public parentData: any,
    private dialogRef: MatDialogRef<any>,
    private _formBuilder: FormBuilder,
    private _testService: TestService,
    public dialog: MatDialog,
    private snackBar: SnackBarService) {
  }

  ngOnInit() {

    this.maxDate = new Date();
    if (!isNullOrUndefined(this.parentData)) {
      this.testQuestion = this.parentData.testQuestion;
      if (this.parentData.isEdit) {
        this.isEdit = true;
      }
      this.initializeForm();
      this.setFormValue();
    } else {
      this.dialogRef.close();
    }

  }

  initializeForm() {
    this.editMarkForm = this._formBuilder.group({
      positiveMark: [this.testQuestion.positiveMark, [Validators.required, Validators.pattern(numberPattern)]],
      negativeMark: [this.testQuestion.negativeMark, [Validators.required, Validators.pattern(numberPattern)]]
    });
  }


  setFormValue() {
    this.editMarkForm.markAsTouched();
    this.markFormGroupTouched(this.editMarkForm)
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
    if (!this.isEdit) {
      this.addMark();
    } else {
      this.updateMark();
    }
  }

  addMark() {
    this.testQuestion.positiveMark = this.editMarkForm.get('positiveMark').value;
    this.testQuestion.negativeMark = this.editMarkForm.get('negativeMark').value;
    this.dialogRef.close(this.testQuestion);
  }

  updateMark() {
    this.showSpinner();
    this.testQuestion.positiveMark = this.editMarkForm.get('positiveMark').value;
    this.testQuestion.negativeMark = this.editMarkForm.get('negativeMark').value;
    this._testService.UpdateTestQuestion(this.testQuestion)
      .subscribe(res => {
        if (res.success) {
          this.snackBar.success("Marks Updated Successfully");
          this.hideSpinner();
          this.dialogRef.close(true);
        } else {
          this.snackBar.success("Error Updating Marks");
          this.hideSpinner();
          this.dialogRef.close();
        }
      })
  }

  showSpinner() { this.isSpinner = true }

  hideSpinner() { this.isSpinner = false }
}
