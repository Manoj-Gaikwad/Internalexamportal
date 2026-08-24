import { Component, OnInit } from '@angular/core';
import { CustomHttpService } from "app/layout/services/custom-http.service";
import { Router } from "@angular/router";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { Inject } from "@angular/core";
import { SnackBarService } from 'app/layout/services/snackbar.service';
import { QuestionComponent } from '../question.component';
import { TestQuestionAddComponent } from '../../test-grid/add-test/test-questions/test-question-add/test-question-add.component';

@Component({
    selector: 'cg-delete-question',
    templateUrl: './delete-question.component.html',
    styleUrls: ['./delete-question.component.scss'],
    standalone: false
})
export class DeleteQuestionComponent implements OnInit {
  Id: any;
  errorMsg: any;

  constructor(private router: Router,
    private http: CustomHttpService,
    @Inject(MAT_DIALOG_DATA) public parentData: any,
    private TestQuestionListdialogRef: MatDialogRef<TestQuestionAddComponent>,
    private QuestionListdialogRef: MatDialogRef<QuestionComponent>,
    private snackBar: SnackBarService) { }


  ngOnInit() {
  }

  deleterow() {
    if (this.parentData.TestId == null) {
      var postData = {
        QuestionId: this.parentData.QuestionId,
        TestID: this.parentData.TestId
      }
      this.http._delete('question/DeleteQuestion/' + this.parentData.QuestionId)
        .subscribe(
          (res: any) => {

            this.QuestionListdialogRef.close(true);
            this.snackBar.success('Question deleted Successfully');
          },
          err => {
            this.QuestionListdialogRef.close();
            this.errorMsg = err.error[""][0];
          }
        );
    } else {
      var postData = {
        QuestionId: this.parentData.QuestionId,
        TestID: this.parentData.TestId
      }
      this.http._post('question/DeleteQuestionFromTest', postData)
        .subscribe(
          (res: any) => {

            this.TestQuestionListdialogRef.close(true);
            this.snackBar.success('Question deleted Successfully');
          },
          err => {
            this.TestQuestionListdialogRef.close();
            this.errorMsg = err.error[""][0];
          }
        );
    }
  }

}
