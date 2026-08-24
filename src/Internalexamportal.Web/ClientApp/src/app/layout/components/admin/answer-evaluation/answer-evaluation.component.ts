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

@Component({
    selector: 'cg-answer-evaluation',
    templateUrl: './answer-evaluation.component.html',
    styleUrls: ['./answer-evaluation.component.scss'],
    providers: [TestService],
    standalone: false
})
export class AnswerEvaluationComponent implements OnInit {

  public form: FormGroup;
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

  constructor(private _testService: TestService, private route: ActivatedRoute,
    public _location: Location, private _formBuilder: FormBuilder,
    public snackBar: SnackBarService, public dialog: MatDialog) { }

  ngOnInit() {

    let id = this.route.snapshot.paramMap.get('id');

    if (id) {
      this.submittedTestId = parseInt(id);
      this.getSubmittedAnswers();
    } else {
      this._location.back();
    }

    this.buildForm();
  }

  buildForm() {
    this.form = this._formBuilder.group({
      id: [this.submittedTestId],
      submittedAnswers: this._formBuilder.array([])
    });
  }

  setSubmittedAnswersFormControl(subjectiveQuestions: Array<SubjectiveQuestion>) {
    this.form.setControl('submittedAnswers', this._formBuilder.array([]));
    subjectiveQuestions.forEach((question, index) => {
      (<FormArray>this.form.get('submittedAnswers')).push(this._formBuilder.group({
        questionId: [question.id],
        marks: [question.submittedAnswer ? question.obtainedMarks : 0, [Validators.required, Validators.max(question.positiveMarks), Validators.min(-question.negativeMarks)]],
        comment: [question.comment]
      }))
    });
  }

  getSubmittedAnswers() {
    this.showSpinner();
    this._testService.GetSubmittedAnswers(this.submittedTestId)
      .subscribe(res => {
        this.testDetails = res;
        this.objectiveQuestions = this.testDetails.objective;
        var subjective = this.testDetails.subjective;
        this.setSubmittedAnswersFormControl(subjective);
        this.subjectiveQuestions = subjective;
        this.calculateObtainedMarks();
        this.hideSpinner();
      }, err => {
        console.error(err);
        this.snackBar.error("Internal Error");
        this._location.back();
      })
  }

  submitEvaluation() {
    if (this.form.valid) {
      this.showSpinner();
      this._testService.SubmitEvaluationMarks(this.form.value)
        .subscribe(res => {
          if (res.success) {
            this.snackBar.success("Marks Stored Successfully.");
            this._location.back();
          }
          this.hideSpinner();
        }, err => {
          console.error(err);
          this.snackBar.error("Internal Error");
          this.hideSpinner();
        })
    } else {
      this.snackBar.error("Please evaluate all the questions before submitting.")
    }
  }

  saveQuestionEvaluation(index) {
    var questionForm = (<FormArray>this.form.get('submittedAnswers')).at(index);
    if (questionForm.valid) {
      this.showSpinner();
      var subjectiveQuestion = questionForm.value;
      subjectiveQuestion.testId = this.submittedTestId;
      this._testService.SaveQuestionEvaluation(subjectiveQuestion)
        .subscribe(res => {
          if (res.success) {
            this.snackBar.success("Marks Stored Successfully.");
          }
          this.hideSpinner();
        }, err => {
          console.error(err);
          this.snackBar.error("Internal Error");
          this.hideSpinner();
        })
    } else {
      this.snackBar.error("Please check marks before saving.")
    }
  }

  calculateObtainedMarks() {
    this.objectiveMarks = this.testDetails.obtainedMark;
    this.calucalteSubjectiveMarks();
  }

  calucalteSubjectiveMarks() {
    this.subjectiveMarks = 0;
    var subjectiveMarks: Array<any> = this.form.value.submittedAnswers;
    subjectiveMarks.forEach((element, index) => {
      if (element.marks && isNumber(element.marks)) {
        if (element.marks >= -this.subjectiveQuestions[index].negativeMarks && element.marks <= this.subjectiveQuestions[index].positiveMarks)
          this.subjectiveMarks += element.marks;
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
    answer = answer.sort();
    question.selectedOptions = question.selectedOptions.sort();
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
