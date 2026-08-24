import { Component, OnInit } from '@angular/core';
import { fuseAnimations } from '@fuse/animations';
import { SharedService } from "app/layout/services/shared.service";
import { timer, Subscription } from 'rxjs';
import { Router } from "@angular/router";
import { FuseConfigService } from "@fuse/services/config.service";
import { ViewEncapsulation } from "@angular/core";
import { FinishModelComponent } from "app/layout/components/candidates/finish-model/finish-model.component";
import { MatDialog } from "@angular/material/dialog";
import { DataService } from '../candidate.service';
import { TestDataService } from 'app/layout/services/test-data-service.service';
import { CandidateService } from 'app/layout/services/candidate.service';
import { fuseFullScreenConfig, TestSetting, maxTestWarningCount, QuestionType, TestType } from 'app/layout/entities/globalConstants';
import { SnackBarService } from 'app/layout/services/snackbar.service';
import { TestService } from 'app/layout/services/test.service';
import { WarningDialogComponent } from '../warning-dialog/warning-dialog.component';
import { UserDataService } from 'app/layout/services/user.service';
import { TestQuestion, ExamQuestion, SubjectiveAnswer } from 'app/layout/entities/models';
import { isNullOrUndefined } from 'util';
import { SubmittedTestService } from 'app/layout/services/submitted-test.service';

function beforeUnloadFunction(e) {
  e.preventDefault();
  e.returnValue = '';
}

@Component({
    selector: 'cg-currentexam',
    templateUrl: './currentexam.component.html',
    styleUrls: ['./currentexam.component.scss'],
    animations: fuseAnimations,
    providers: [TestService, SubmittedTestService],
    standalone: false
})

export class CurrentexamComponent implements OnInit {

  public isSpinner: boolean = false;

  public questionTypeEnum = QuestionType;

  testId: any;
  testName: string;
  mcqRadioGroup: any;
  tfRadioGroup: any;
  attempQuestion: any;
  selectedValue: any;
  subjectiveUserAnswer: string;
  subjectiveAnswers: Array<SubjectiveAnswer> = [];
  multipleChoice: any[] = ['A', 'B', 'C', 'D'];
  trueFalseType: any[] = ['A', 'B'];
  public multipleQuestion: boolean = false;
  public truefalse: boolean = false;
  public Subjective: boolean = false;
  questionSet: Array<ExamQuestion>;
  currentIndex: ExamQuestion;
  totalLength: number;
  reviewLength: number = 0;
  skipLength: any;
  attemptedLength: any;
  timeLeft: number;
  subscribeTimer: any;
  totalTime: any;
  index = 0;
  skipQuestion = [];
  reviewQuestion = [];
  savedAnswers: any = [];
  totalQuestion: any;
  errorMsg: string;
  totalMarks: number = 0;
  timerWarn: boolean = false;
  timeTaken: any;
  examData: any;
  testWarnings: number = 0;
  user: any;
  userPhoto: string;
  clientName: string = "Exam Portal";
  focusTimer: NodeJS.Timer;
  examTimer: Subscription;

  constructor(private _candidateService: CandidateService,
    private router: Router,
    public dialog: MatDialog,
    private snackBar: SnackBarService,
    public dataService: DataService,
    private _sharedService: SharedService,
    private testDataService: TestDataService,
    private _fuseConfigService: FuseConfigService,
    private _userDataService: UserDataService,
    private _submittedTestService: SubmittedTestService) {
    this._fuseConfigService.config = fuseFullScreenConfig;
  }

  ngOnInit() {
    this.getUserDetails();
    this.user = this._sharedService.getUserFromToken();
    window.addEventListener('beforeunload', beforeUnloadFunction);
    this.testId = this.testDataService.testId;
    this.testName = this.testDataService.testName;
    if (this.testId == 0) {
      this.router.navigate(['./layout/candidates/my-test']);
      return;
    }

    this.getQuestionSet();
  }

  getUserDetails() {
    this._userDataService.getUserDetails();
    this._userDataService.user
      .subscribe(user => {
        this.userPhoto = user.photo;
        this.clientName = user.clientName;
      });
  }

  getQuestionSet() {
    this.showSpinner();
    const source = timer(0, 1000);

    var request = {
      id: this.testId,
      isPendingTest: this.testDataService.isPendingTest,
      submittedTestId: this.testDataService.submittedTestId
    }

    this._candidateService.GetQuestionPaper(request)
      .subscribe(res => {
        let response: any = res;
        this.checkWindowFocus();

        if (this.testDataService.isPendingTest) {
          if (!isNullOrUndefined(res.subjectiveAnswers))
            this.subjectiveAnswers = res.subjectiveAnswers;
        }
        this.questionSet = response.questionsList;	 // FILL THE ARRAY WITH DATA.

        this.totalLength = this.questionSet.length;
        this.currentIndex = response.questionsList[0];
        this.getCurrentindex();

        this.timeLeft = this.testDataService.testDuration * 60;
        this.totalTime = this.convertSeconds(this.timeLeft);

        if (this.testDataService.isPendingTest) {
          this.timeLeft = this.timeLeft - this.testDataService.timeTaken;
        }

        let warnTime = (this.testDataService.testDuration * 60) * 0.25;
        this.examTimer = source.subscribe(val => {

          this.timeTaken = this.testDataService.timeTaken + val;
          this.subscribeTimer = this.convertSeconds(this.timeLeft - val);
          if (this.subscribeTimer == this.convertSeconds(warnTime)) {
            this.timerWarn = true;
          }
          if (isNullOrUndefined(this.currentIndex.timeTaken)) {
            this.currentIndex.timeTaken = 1;
          } else {
            this.currentIndex.timeTaken++;
          }
          if (this.subscribeTimer == "Time Over") {
            this.examTimer.unsubscribe();
            this.submitExam();
          }
        });
        this.hideSpinner();

      },
        err => {
          console.log(err.message);
          this.hideSpinner();
        }
      );
  }

  convertSeconds(s) {
    var min = Math.floor(s / 60);
    var sec = s % 60;
    if (min < 0 && sec < 0) {
      return "Time Over";
    }
    else {
      const seconds = sec < 10 ? '0' + sec.toString() : sec
      const minutes = min < 10 ? '0' + min.toString() : min
      return minutes + ': ' + seconds;
    }
  }

  //To Check Question Type :- MCQ = 1, True/False = 2, Subjective = 3
  public getCurrentindex() {
    if (this.currentIndex.question.questionTypeId == QuestionType.MultipleChoice) {
      this.multipleQuestion = true;
      this.Subjective = false;
      this.truefalse = false;
    }
    else if (this.currentIndex.question.questionTypeId == QuestionType.TrueFalse) {
      this.truefalse = true;
      this.multipleQuestion = false;
      this.Subjective = false;
    } else {
      this.Subjective = true;
      this.truefalse = false;
      this.multipleQuestion = false;
    }

    if (this.Subjective) {
      var answer = this.subjectiveAnswers.find(prop => prop.questionId == this.currentIndex.questionId);
      if (answer) {
        this.subjectiveUserAnswer = answer.answerText;
      } else {
        this.subjectiveUserAnswer = "";
      }
    }
  }

  changeSelection(position, event) {
    if (!this.currentIndex.isMultiSelect) {

      var checked = event.checked;
      if (isNullOrUndefined(checked)) { checked = event.target.checked; }

      this.currentIndex.question.questionOption.forEach((option, index) => {
        if (position != index) {
          option.isChecked = false;
        }
      });
    }
    this.currentIndex.answered = this.currentIndex.question.questionOption.filter(prop => prop.isChecked).length > 0;
  }

  onRightClick() {
    return false;
  }

  saveAndNext() {

    if (!this.Subjective && this.currentIndex.answered) {

      this.saveSubmittedQuestion();
      this.nextQuestion();

    } else if (this.Subjective) {

      this.saveSubmittedQuestion();
      this.saveSubjectiveAnswer();
      this.nextQuestion();

    } else {
      this.snackBar.open('Select answer or skip question');
    }
  }

  nextQuestion() {
    this.currentIndex.answered = true;
    if (this.index != this.totalLength - 1) {
      this.index++;
      this.currentIndex = this.questionSet[this.index];
      this.getCurrentindex();
    }
  }

  previous() {

    //Save typed answer while changing question
    if (this.Subjective) {
      this.saveSubjectiveAnswer();
    }

    if (this.index != 0) {
      this.index--;
    }
    this.currentIndex = this.questionSet[this.index];
    this.getCurrentindex();
  }

  saveSubjectiveAnswer() {
    let position: number = this.subjectiveAnswers.findIndex(prop => prop.questionId == this.currentIndex.questionId);

    if (position >= 0) {
      this.subjectiveAnswers[position] = {
        questionId: this.currentIndex.questionId,
        answerText: this.subjectiveUserAnswer,
        timeTaken: this.currentIndex.timeTaken
      }
    } else {
      this.subjectiveAnswers.push({
        questionId: this.currentIndex.questionId,
        answerText: this.subjectiveUserAnswer,
        timeTaken: this.currentIndex.timeTaken
      })
    }
  }

  goToQuestion(id: number) {

    //Save typed answer while changing question
    if (this.Subjective) {
      this.saveSubjectiveAnswer();
    }

    this.index = id;
    this.currentIndex = this.questionSet[this.index];
    this.getCurrentindex();
  }

  review() {
    if (this.currentIndex.Marked == true) {
      this.reviewLength--;
      this.currentIndex.Marked = false;
    } else {
      this.reviewLength++;
      this.currentIndex.Marked = true;
    }
    this.getCurrentindex();
  }

  skip() {
    if (this.index != this.totalLength - 1) {
      this.skipQuestion.push({ 'questionId': this.currentIndex.id });
      this.skipLength = this.skipQuestion.length;
      this.currentIndex.NotAnswered = true;
      this.index++;
    }
    this.currentIndex = this.questionSet[this.index];
    this.getCurrentindex();
  }

  getSavedAnswersArray(): any[] {
    this.savedAnswers = [];
    this.questionSet.forEach(element => {
      if (element.answered && element.question.questionTypeId != QuestionType.Subjective) {
        this.savedAnswers.push({
          questionId: element.questionId,
          timeTaken: element.timeTaken,
          answers: element.question.questionOption.filter(option => option.isChecked).map(option => option.id)
        });
      }
    });
    return this.savedAnswers;
  }

  saveSubmittedQuestion() {

    this.showSpinner();
    var submittedQuestion = {
      timeTaken: this.timeTaken,
      submittedTestId: this.testDataService.submittedTestId,
      questionType: this.currentIndex.question.questionTypeId,
      subjectiveAnswer: null,
      objectiveAnswer: null,
    };

    if (submittedQuestion.questionType == QuestionType.Subjective) {

      submittedQuestion.subjectiveAnswer = {
        questionId: this.currentIndex.questionId,
        answerText: this.subjectiveUserAnswer,
        timeTaken: this.currentIndex.timeTaken
      }

    } else {

      submittedQuestion.objectiveAnswer = {
        questionId: this.currentIndex.questionId,
        answers: this.currentIndex.question.questionOption.filter(option => option.isChecked).map(option => option.id),
        timeTaken: this.currentIndex.timeTaken
      }

    }

    this._submittedTestService.saveSubmittedQuestion(submittedQuestion)
      .subscribe((res: any) => {
        this.hideSpinner();
        this.snackBar.success('Answer saved successfully.');
      }, (err: any) => {
        this.hideSpinner();
        this.snackBar.error('Error saving answer');
        console.log(err);
      })
  }

  getSubmitExamData() {
    let userOptions = this.getSavedAnswersArray();
    let attemptedQuestions = this.questionSet.filter(prop => prop.answered).length;
    this.examData = {
      userId: "",
      testId: this.testId,
      attemptedQuestion: attemptedQuestions,
      skippedQuestion: this.questionSet.length - attemptedQuestions,
      reviewedQuestion: this.reviewLength,
      userOptions: userOptions,
      subjectiveAnswers: this.subjectiveAnswers,
      timeTaken: this.timeTaken,
      submittedTestId: this.testDataService.submittedTestId
    }
    // console.log(this.examData);
  }

  finishExam() {
    this.getSubmitExamData();
    const dialogInstance = this.dialog.open(FinishModelComponent, {
      disableClose: true,
      panelClass: ['app-no-padding-dialog'],
      width: '600px',
      data: this.examData
    });

    dialogInstance.afterClosed().subscribe(examClosedResult => {
      if (examClosedResult) {
        this.submitExam();
      }
    });
  }

  submitExam() {
    this.showSpinner();
    this.getSubmitExamData();
    this.dialog.closeAll();
    clearInterval(this.focusTimer);
    this.examTimer.unsubscribe();
    if (this.examData != null) {
      this._candidateService.SubmitTest(this.examData)
        .subscribe(
          (res) => {
            this.testDataService.submittedTestId = res.submittedTestId;
            window.removeEventListener('beforeunload', beforeUnloadFunction);
            if (this.testDataService.isTestSettingApplied(TestSetting.DisplayResult) && this.testDataService.testTypeId == TestType.Objective) {
              this.router.navigate(['./layout/candidates/testresult']);
            } else {
              this.router.navigate(['./layout/candidates/success']);
            }
            this.hideSpinner();
          },
          err => {
            this.errorMsg = err;
            this.hideSpinner();
          }
        );
    } else if (this.examData == null) {
      window.removeEventListener('beforeunload', beforeUnloadFunction);
      this.router.navigate(['./layout/candidates/testresult']);
    }
  }

 
  
  logWhenPageHidden(): void {
    this.openWarningDialog();
  }

  openWarningDialog() {

    this.testWarnings++;

    const dialogRef = this.dialog.open(WarningDialogComponent, {
      width: '600px',
      panelClass: ['app-no-padding-dialog'],
      disableClose: true,
      restoreFocus: false,
      data: {
        count: this.testWarnings
      }
    });

    dialogRef.afterClosed().subscribe(() => {
      if (this.testWarnings >= maxTestWarningCount) {
        this.submitExam();
      } else {
        this.checkWindowFocus();
      }
    })
  }

  checkWindowFocus() {
    if (this.testDataService.isTestSettingApplied(TestSetting.WindowMinimiseWarning)) {
      this.focusTimer = setInterval(() => {
        if (!document.hasFocus()) {
          this.openWarningDialog();
          clearInterval(this.focusTimer)
        }
      }, 1000);
    }
  }

  onTabPress(event) {
    this.subjectiveUserAnswer = this.subjectiveUserAnswer + "   ";
    event.preventDefault();
  }

  showSpinner() { this.isSpinner = true }

  hideSpinner() { this.isSpinner = false }
}

