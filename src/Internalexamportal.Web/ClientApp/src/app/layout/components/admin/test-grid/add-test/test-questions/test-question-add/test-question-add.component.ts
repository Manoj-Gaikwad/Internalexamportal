import { Component, OnInit, Inject } from '@angular/core';
import { HttpErrorResponse } from "@angular/common/http";
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SelectionModel } from "@angular/cdk/collections";
import { ViewChild } from "@angular/core";
import { FormBuilder, Validators } from "@angular/forms";
import { FormGroup } from "@angular/forms";
import { QuestionService } from 'app/layout/services/question.service';
import { TestService } from 'app/layout/services/test.service';
import { SnackBarService } from 'app/layout/services/snackbar.service';
import { QuestionAddComponent } from 'app/layout/components/admin/question/question-add/question-add.component';
import { EditMarksComponent } from '../edit-marks/edit-marks.component';
import { QuestionType } from 'app/layout/entities/globalConstants';

@Component({
    selector: 'cg-test-question-add',
    templateUrl: './test-question-add.component.html',
    styleUrls: ['./test-question-add.component.scss'],
    providers: [QuestionService, TestService],
    standalone: false
})

export class TestQuestionAddComponent implements OnInit {

  public isSpinner: boolean = false;

  SubjectForm: FormGroup;
  QuestionTypeId: any = null;
  SubjectTopicId: any = null;
  SubjectId: any = null;
  subjectTopics: any;
  QuestionType: any;
  subjects: any;

  testId: any;
  testDetails: any = {
    totalQuestion: 0,
    totalMark: 0,
    testTypeId: 0,
  }
  totalMarks: number = 0;
  totalQuestions: number = 0;
  newTotalMarks: number = 0;
  newTotalQuestions: number = 0;


  public dataSource = new MatTableDataSource<any>();
  public selection = new SelectionModel<any>(true, []);
  arrays: any;
  displayedColumns: string[] = ['Select', 'id', 'description', 'positiveMark', 'negativeMark', 'action'];
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(private dialogRef: MatDialogRef<any>, @Inject(MAT_DIALOG_DATA) public parentData: any,
    private _testService: TestService, public dialog: MatDialog,
    private snackBar: SnackBarService, private _formBuilder: FormBuilder,
    private _questionService: QuestionService) { }

  ngOnInit() {
    this.buildForm();

    if (this.parentData) {
      this.testId = this.parentData.testId;
      this.testDetails = this.parentData.testDetails;
      this.totalMarks = this.parentData.totalMarks;
      this.totalQuestions = this.parentData.totalQuestions;
      this.updateTotalMarksWithQuestions()
    }

    this.getAllSubjects();
    this.getQuestionTypes();
  }

  buildForm() {
    this.SubjectForm = this._formBuilder.group({
      SubjectId: [this.SubjectId, Validators.required],
      SubjectTopicId: [this.SubjectTopicId, Validators.required],
      QusetionTypeId: [this.QuestionTypeId, Validators.required]
    });
  }

  getAllSubjects() {
    this.showSpinner();
    this._questionService.GetAllSubjects().subscribe(
      data => {
        this.subjects = data;
        this.hideSpinner();
      },
      (err: HttpErrorResponse) => {
        console.log(err.message);
      }
    );
  }

  Gettopic(subject) {
    this.showSpinner();
    this.SubjectId = subject.value;
    this._questionService.GetTopic(this.SubjectId).subscribe(
      data => {
        this.subjectTopics = data;
        this.hideSpinner();
      },
      (err: HttpErrorResponse) => {
        console.log(err.message);
      }
    )
  }

  getQuestionTypes() {
    this.showSpinner();
    this._questionService.GetQuestionTypes().subscribe(
      (data: Array<any>) => {
        var type = this.testDetails.testTypeId;
        switch (type) {
          case 1:
            this.QuestionType = data.filter(prop => prop.id != QuestionType.Subjective);
            break;
          case 2:
            this.QuestionType = data.filter(prop => prop.id == QuestionType.Subjective);
            break;
          default:
            this.QuestionType = data;
            break;
        }

        this.hideSpinner();
      },
      (err: HttpErrorResponse) => {
        console.log(err.message);
      }
    );
  }

  applyFilter(value: string) {
    this.dataSource.filter = value;
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.dataSource.data.forEach(row => this.selection.select(row));
  }

  masterClearSelection() {
    this.selection.clear();
  }

  updateTotalMarksWithQuestions() {
    this.newTotalMarks = this.totalMarks;
    this.newTotalQuestions = this.totalQuestions;
    this.selection.selected.forEach(element => {
      this.newTotalMarks += +element.positiveMark;
      this.newTotalQuestions++;
    });
  }

  AddQuestion() {
    this.showSpinner();
    var questions = this.selection.selected.map(prop => ({
      testId: this.testId,
      questionId: prop.id,
      positiveMark: prop.positiveMark,
      negativeMark: prop.negativeMark
    }));
    var TestId = this.testId;
    var questionAns = { questions, TestId }
    this._testService.AddTestQuestion(questionAns)
      .subscribe(
        () => {
          this.hideSpinner();
          this.dialogRef.close(true);
          this.snackBar.success('Add questions in  test successfully');
        });
  }

  onSubmit() {
    this.showSpinner();
    this.selection.clear();
    var Form = this.SubjectForm.value;
    this.SubjectId = Form.SubjectId;
    this.SubjectTopicId = Form.SubjectTopicId;
    this.QuestionTypeId = Form.QusetionTypeId;

    this._testService.GetFilteredQuestionsForTest(this.testId, this.SubjectId, this.SubjectTopicId, this.QuestionTypeId)
      .subscribe(
        data => {
          this.hideSpinner();
          this.arrays = data;
          this.populateQuestions();
        },
        (err: HttpErrorResponse) => {
          console.log(err.message);
        }
      );
  }

  populateQuestions() {
    this.dataSource = new MatTableDataSource(this.arrays);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.updateTotalMarksWithQuestions();
  }

  EditQuestion(row) {
    var index = this.arrays.indexOf(row);

    const dialogRef = this.dialog.open(EditMarksComponent, {
      disableClose: true,
      data: {
        testQuestion: row,
        isEdit: false
      }
    });

    dialogRef.afterClosed()
      .subscribe((result) => {
        if (result) {
          this.arrays[index] = result;
          this.populateQuestions();
          this.selection.select(result);
        }
      })
  }


  showSpinner() { this.isSpinner = true }

  hideSpinner() { this.isSpinner = false }

}
