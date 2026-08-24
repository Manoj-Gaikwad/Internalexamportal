import { Component, OnInit } from '@angular/core';
import { TestService } from 'app/layout/services/test.service';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { SubmittedTest, ObjectiveQuestion, SubjectiveQuestion } from 'app/layout/entities/models';
import { QuestionType, numberPattern } from 'app/layout/entities/globalConstants';
import { FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { isNumber } from 'lodash';
import { SnackBarService } from 'app/layout/services/snackbar.service';
import { MatDialog } from '@angular/material/dialog';
import { AnswerSheetService } from 'app/layout/services/answer-sheet.service';

@Component({
    selector: 'cg-answer-sheet',
    templateUrl: './answer-sheet.component.html',
    styleUrls: ['./answer-sheet.component.scss'],
    providers: [TestService],
    standalone: false
})
export class AnswerSheetComponent implements OnInit {

  public isSpinner: boolean;
  public submittedTestId: number;
  public testDetails: SubmittedTest;
  public objectiveQuestions: Array<ObjectiveQuestion>;
  public subjectiveQuestions: Array<SubjectiveQuestion>;
  public mcqArray: Array<string> = ['A', 'B', 'C', 'D'];

  public obtainedMarks: number = 0;
  public objectiveMarks: number = 0;
  public subjectiveMarks: number = 0;
  public answer: string;

  constructor(private _testService: TestService, private _answerSheetService: AnswerSheetService,
    public _location: Location, private _formBuilder: FormBuilder,
    public snackBar: SnackBarService, public dialog: MatDialog) { }

  ngOnInit() {

    let id = this._answerSheetService.submittedTestId;

    if (id) {
      this.submittedTestId = id;
      this.getSubmittedAnswers();
    } else {
      this._location.back();
    }

  }

  getSubmittedAnswers() {
    this.showSpinner();
    this._testService.GetSubmittedAnswers(this.submittedTestId)
      .subscribe(res => {
        this.testDetails = res;
        this.objectiveQuestions = this.testDetails.objective;
        this.subjectiveQuestions = this.testDetails.subjective;
        //this.calculateObtainedMarks();
        this.hideSpinner();
      }, err => {
        console.error(err);
        this.snackBar.error("Internal Error");
        this._location.back();
      })
  }

  calculateObtainedMarks() {
    this.objectiveMarks = this.testDetails.obtainedMark;
    this.calucalteSubjectiveMarks();
  }

  calucalteSubjectiveMarks() {
    this.subjectiveMarks = 0;
    this.subjectiveQuestions.forEach((element, index) => {
      if (element.obtainedMarks && isNumber(element.obtainedMarks)) {
        this.subjectiveMarks += element.obtainedMarks;
      }
    });
    this.obtainedMarks = this.objectiveMarks + this.subjectiveMarks;
  }

  isUserSelectedAnswer(question: ObjectiveQuestion, optionId: number): boolean {
    if (question.selectedOptions.find(prop => prop == optionId)) {
      return true;
    } else {
      return false;
    }
  }

  checkAnswer(question: ObjectiveQuestion): boolean {
    var answer = question.questionOptions.filter(prop => prop.isAnswer).map(prop => prop.id);

    var isCorrect = (answer.length === question.selectedOptions.length) && answer.every(function (element, index) {
      return element === question.selectedOptions[index];
    });

    return isCorrect;
  }

  viewAnswer(question: SubjectiveQuestion, dialog) {
    this.answer = question.questionOption.optionText;
    const dialogRef = this.dialog.open(dialog, {
      position: { top: "5%" },
      panelClass: ['app-no-padding-dialog'],
      restoreFocus: true
    });
  }

  showSpinner() { this.isSpinner = true }

  hideSpinner() { this.isSpinner = false }
}
