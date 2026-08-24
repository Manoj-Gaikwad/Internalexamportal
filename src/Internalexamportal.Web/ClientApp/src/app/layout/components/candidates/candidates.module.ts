import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CandidatesComponent } from 'app/layout/components/candidates/candidates.component';
import { MyTestComponent } from './my-test/my-test.component';
import { ReportComponent } from './report/report.component';
import { RouterModule, Routes } from '@angular/router';
import { CurrentexamComponent } from './currentexam/currentexam.component';
import { ExamruleComponent } from './examrule/examrule.component';
import { ExamresultComponent } from './examresult/examresult.component';
import { ActivationcodeComponent } from "app/layout/components/candidates/activationcode/activationcode.component";
import { UpcomingTestsComponent } from './upcoming-tests/upcoming-tests.component';
import { SharedModule } from 'app/shared/shared.module';
import { CandidateService } from 'app/layout/services/candidate.service';
import { CandidateProfileComponent } from './candidate-profile/candidate-profile.component';
import { SubmitSuccessComponent } from './submit-success/submit-success.component';
import { WarningDialogComponent } from './warning-dialog/warning-dialog.component';
import { AnswerSheetComponent } from './answer-sheet/answer-sheet.component';
import { AnswerSheetService } from 'app/layout/services/answer-sheet.service';


const appRoutes: Routes = [
  {
    path: 'upcoming',
    component: UpcomingTestsComponent
  },
  {
    path: 'my-test',
    component: MyTestComponent
  },
  {
    path: 'report',
    component: ReportComponent
  },
  {
    path: 'answer-sheet',
    component: AnswerSheetComponent
  },
  {
    path: 'activation',
    component: ActivationcodeComponent
  },
  {
    path: 'examrule',
    component: ExamruleComponent
  },
  {
    path: 'currentexam',
    component: CurrentexamComponent
  },
  {
    path: 'success',
    component: SubmitSuccessComponent
  },
  {
    path: 'testresult',
    component: ExamresultComponent
  },
  {
    path: 'profile',
    component: CandidateProfileComponent
  }
]

@NgModule({
  declarations: [
    UpcomingTestsComponent,
    MyTestComponent,
    ReportComponent,
    ActivationcodeComponent,
    ExamruleComponent,
    CurrentexamComponent,
    CandidatesComponent,
    ExamresultComponent,
    CandidateProfileComponent,
    SubmitSuccessComponent,
    WarningDialogComponent,
    AnswerSheetComponent
  ],
  imports: [
    SharedModule,
    CommonModule,
    RouterModule.forChild(appRoutes),
  ],

  providers: [CandidateService, AnswerSheetService]
})
export class CandidatesModule { }
