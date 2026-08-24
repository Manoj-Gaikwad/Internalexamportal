import { Component, OnInit, Input } from '@angular/core';
import { HttpErrorResponse } from "@angular/common/http";
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { ViewChild } from "@angular/core";
import { TestService } from 'app/layout/services/test.service';
import { QuestionAddComponent } from '../../../question/question-add/question-add.component';
import { DeleteQuestionComponent } from '../../../question/delete-question/delete-question.component';
import { TestQuestionAddComponent } from './test-question-add/test-question-add.component';
import { EditMarksComponent } from './edit-marks/edit-marks.component';

@Component({
    selector: 'test-questions',
    templateUrl: './test-questions.component.html',
    styleUrls: ['./test-questions.component.scss'],
    providers: [TestService],
    standalone: false
})
export class TestQuestionsComponent implements OnInit {

  @Input() testId: any;
  @Input() testDetails: any = {
    totalQuestion: 0,
    totalMark: 0,
    testTypeId:0,
  }
  totalMarks: number = 0;
  totalQuestions: number = 0;

  public dataSource = new MatTableDataSource<any>();
  arrays: any;
  displayedColumns: string[] = ['id', 'description', 'positiveMark', 'negativeMark', 'action'];
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(private _testService: TestService, public dialog: MatDialog,) { }

  ngOnInit() {
    this.getTestQuestion();
  }

  getTestQuestion() {
    this._testService.GetTestQuestions(this.testId)
      .subscribe(data => {
        this.arrays = data;
        this.dataSource = new MatTableDataSource(this.arrays);
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
        this.dataSource.paginator.firstPage();
        this.calculateTotalMarks();
      },
        (err: HttpErrorResponse) => {
          console.log(err.message);
        });
  }

  calculateTotalMarks(){
    this.totalQuestions = this.arrays.length;
    this.totalMarks = 0;
    this.arrays.forEach(element => {
      this.totalMarks += parseInt(element.positiveMark);
    });
  }


  applyFilter(value: string) {
    this.dataSource.filter = value;

  }

  EditQuestion(row) {
    const dialogRef = this.dialog.open(EditMarksComponent, {
      disableClose: true,
      data: {
        testQuestion: row,
        isEdit: true
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.calculateTotalMarks();
        //this.getTestQuestion();
      }
    });
  }

  openDialog(row) {

    const dialogRef = this.dialog.open(DeleteQuestionComponent, {
      disableClose: true,
      panelClass: ['app-no-padding-dialog'],
      data: {
        QuestionId: row.questionId,
        TestId: this.testId
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result)
        this.getTestQuestion();
    });

  }

  openAddTestQuestionDialog() {
    const dialogRef = this.dialog.open(TestQuestionAddComponent, {
      restoreFocus: false,
      disableClose: true,
      width: '80%',
      data: {
        testId: this.testId,
        totalMarks: this.totalMarks,
        totalQuestions: this.totalQuestions,
        testDetails: this.testDetails
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result)
        this.getTestQuestion();
    });
  }

}
