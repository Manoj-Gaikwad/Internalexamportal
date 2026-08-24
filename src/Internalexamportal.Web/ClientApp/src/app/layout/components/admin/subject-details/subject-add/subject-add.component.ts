import { Component, OnInit, Inject } from '@angular/core';
import { HttpErrorResponse } from "@angular/common/http";
import { HttpClient } from "@angular/common/http";
import { Router } from "@angular/router";
import { CustomHttpService } from "app/layout/services/custom-http.service";
import { FormBuilder, Validators, FormControl, FormGroup } from "@angular/forms";
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { QuestionService } from 'app/layout/services/question.service';
import { SnackBarService } from 'app/layout/services/snackbar.service';

@Component({
    selector: 'cg-subject-add',
    templateUrl: './subject-add.component.html',
    styleUrls: ['./subject-add.component.scss'],
    providers: [QuestionService],
    standalone: false
})
export class SubjectAddComponent implements OnInit {
  public isSpinner: boolean = false;
  id: number = 0;
  isEdit: boolean = false;
  dialogTitle: string = "Add Subject";
  errorMsg: any;
  SubjectForm: FormGroup;
  arrays: any;
  dataSource: any;
  displayedColumns: string[] = ['SrNo', 'SubjectName', 'Action'];
  constructor(private dialogRef: MatDialogRef<any>, private _formBuilder: FormBuilder,
    private _questionService: QuestionService, @Inject(MAT_DIALOG_DATA) public parentData: any,
    private snackBar: SnackBarService) { }

  ngOnInit() {
    if (this.parentData == null) {
      this.buildForm();
    } else {
      this.id = this.parentData.id;
      this.isEdit = true;
      this.buildForm();
      this.getSubjectDetails();
      this.dialogTitle = "Edit Subject";
    }
  }

  buildForm() {
    this.SubjectForm = this._formBuilder.group({
      id: [this.id],
      SubjectName: [null, Validators.required]
    });
  }

  getSubjectDetails() {
    this.showSpinner();
    this._questionService.GetSubjectDetails(this.id)
      .subscribe(result => {
        this.hideSpinner();
        this.SubjectForm.get('SubjectName').setValue(result.subjectName);
      }, error => {
        console.error("Error getting subject details");
      })
  }

  addSubject(subject) {
    this.showSpinner();
    this._questionService.AddSubject(subject)
      .subscribe(
        (res: any) => {
          this.hideSpinner();
          if (res.success) {
            this.snackBar.success('Subject Added Successfully');
            this.dialogRef.close(true);
          } else {
            this.snackBar.error(res.message);
          }
        },
        err => {
          this.hideSpinner();
          this.snackBar.error('Adding subject failed');
        }
      );
  }

  updateSubject(subject) {
    this.showSpinner();
    this._questionService.EditSubject(subject)
      .subscribe(
        (res: any) => {
          this.hideSpinner();
          if (res.success) {
            this.snackBar.success('Subject Updated Successfully');
            this.dialogRef.close(true);
          } else {
            this.snackBar.error(res.message);
          }
        },
        err => {
          this.hideSpinner();
          this.snackBar.error('Updating subject failed');
        }
      );
  }

  onSubmit() {
    var SubjectForm = this.SubjectForm.value;
    if (this.isEdit) {
      this.updateSubject(SubjectForm);
    } else {
      this.addSubject(SubjectForm);
    }
  }

  showSpinner() { this.isSpinner = true }

  hideSpinner() { this.isSpinner = false }
}
