import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { CandidateListComponent } from './candidate-list/candidate-list.component';
import { SharedModule } from 'app/shared/shared.module';
import { SubjectDetailsComponent } from './subject-details/subject-details.component';
import { QuestionComponent } from "app/layout/components/admin/question/question.component";
import { ImportQuestionComponent } from './import-question/import-question.component';
import { TestResultComponent } from './test-result/test-result.component';
import { AdminComponent } from './admin.component';
import { CandidateDetailComponent } from '../candidates/candidate-detail/candidate-detail.component';
import { TestGridComponent } from './test-grid/test-grid.component';
import { DeleteCandidateComponent } from '../candidates/delete-candidate/delete-candidate.component';
import { FinishModelComponent } from '../candidates/finish-model/finish-model.component';
import { AdminManageComponent } from './admin-manage/admin-manage.component';
import { Papa } from 'ngx-papaparse';
import { ImportCandidateListComponent } from './import-candidate-list/import-candidate-list.component';
import { AdminProfileComponent } from './admin-profile/admin-profile.component';
import { AdminPermissionsComponent } from './admin-permissions/admin-permissions.component';
import { AddRoleComponent } from './admin-permissions/add-role/add-role.component';
import { CandidateGroupsComponent } from './candidate-list/candidate-groups/candidate-groups.component';
import { AddGroupComponent } from './candidate-list/candidate-groups/add-group/add-group.component';
import { EditImportCandidateComponent } from './import-candidate-list/edit-import-candidate/edit-import-candidate.component';
import { AddTestComponent } from './test-grid/add-test/add-test.component';
import { CandidateTestsComponent } from './candidate-list/candidate-tests/candidate-tests.component';
import { TestQuestionsComponent } from './test-grid/add-test/test-questions/test-questions.component';
import { DeleteTestComponent } from './test-grid/delete-test/delete-test.component';
import { AddCandidateComponent } from './candidate-list/add-candidate/add-candidate.component';
import { AddAdminComponent } from './admin-manage/add-admin/add-admin.component';
import { QuestionAddComponent } from './question/question-add/question-add.component';
import { DeleteQuestionComponent } from './question/delete-question/delete-question.component';
import { SubjectAddComponent } from './subject-details/subject-add/subject-add.component';
import { EditSubjectComponent } from './subject-details/edit-subject/edit-subject.component';
import { TopicAddEditComponent } from './subject-details/topic-add-edit/topic-add-edit.component';
import { EmailTrackComponent } from './test-grid/add-test/email-track/email-track.component';
import { DeletesubjectComponent } from './subject-details/deletesubject/deletesubject.component';
import { TestInstructionsComponent } from './test-instructions/test-instructions.component';
import { AddInstructionComponent } from './test-instructions/add-instruction/add-instruction.component';
import { DeleteInstructionComponent } from './test-instructions/delete-instruction/delete-instruction.component';
import { ViewInstructionsComponent } from './test-instructions/view-instructions/view-instructions.component';
import { TestQuestionAddComponent } from './test-grid/add-test/test-questions/test-question-add/test-question-add.component';
import { EditMarksComponent } from './test-grid/add-test/test-questions/edit-marks/edit-marks.component';
import { AnswerEvaluationComponent } from './answer-evaluation/answer-evaluation.component';
import { ClientManageComponent } from './client-manage/client-manage.component';
import { AddEditClientComponent } from './client-manage/add-edit-client/add-edit-client.component';
import { RoleGuard } from 'app/layout/security/role-guard';
import { PermissionEnum } from 'app/layout/entities/permission.enum';
import { DeleteClientComponent } from './client-manage/delete-client/delete-client.component';
import { DeleteGroupComponent } from './candidate-list/candidate-groups/delete-group/delete-group.component';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';

import { QuillModule } from 'ngx-quill';
const appRoutes: Routes = [
  {
    path: 'dashboard',
    component: AdminComponent,
    data: { permissionKey: PermissionEnum.ViewDashboard }
  },
  {
    path: 'question',
    component: QuestionComponent,
    canActivate: [RoleGuard],
    data: { permissionKey: PermissionEnum.ViewQuestion }
  },
  {
    path: 'add-test',
    component: AddTestComponent,
    canActivate: [RoleGuard],
    data: { permissionKey: PermissionEnum.AddEditTest }
  },
  {
    path: 'edit-test/:testId',
    component: AddTestComponent,
    canActivate: [RoleGuard],
    data: { permissionKey: PermissionEnum.AddEditTest }
  },
  {
    path: 'tests',
    component: TestGridComponent,
    canActivate: [RoleGuard],
    data: { permissionKey: PermissionEnum.ViewTest }
  },
  {
    path: 'tests/test-result/:testId',
    component: TestResultComponent,
    canActivate: [RoleGuard],
    data: { permissionKey: PermissionEnum.ViewTest }
  },
  {
    path: 'tests/evaluate/:id',
    component: AnswerEvaluationComponent,
    canActivate: [RoleGuard],
    data: { permissionKey: PermissionEnum.ViewTest }
  },
  {
    path: 'test-instructions',
    component: TestInstructionsComponent,
    canActivate: [RoleGuard],
    data: { permissionKey: PermissionEnum.ViewTest }
  },
  {
    path: 'candidate-list',
    component: CandidateListComponent,
    canActivate: [RoleGuard],
    data: { permissionKey: PermissionEnum.ViewCandidate }
  },
  {
    path: 'candidate-groups',
    component: CandidateGroupsComponent,
    canActivate: [RoleGuard],
    data: { permissionKey: PermissionEnum.ViewCandidate }
  },
  {
    path: 'subject-details',
    component: SubjectDetailsComponent,
    canActivate: [RoleGuard],
    data: { permissionKey: PermissionEnum.ViewSubject }
  },
  {
    path: 'subject-edit',
    component: EditSubjectComponent,
    canActivate: [RoleGuard],
    data: { permissionKey: PermissionEnum.AddEditSubject }
  },
  {
    path: 'import-question',
    component: ImportQuestionComponent,
    canActivate: [RoleGuard],
    data: { permissionKey: PermissionEnum.ImportQuestion }
  },
  {
    path: 'candidate-detail',
    component: CandidateDetailComponent
  },
  {
    path: 'manage',
    component: AdminManageComponent,
    canActivate: [RoleGuard],
    data: { permissionKey: PermissionEnum.ViewAdmin }
  },
  {
    path: 'profile',
    component: AdminProfileComponent
  },
  {
    path: 'permissions',
    component: AdminPermissionsComponent,
    canActivate: [RoleGuard],
    data: { permissionKey: PermissionEnum.RolePermissions }
  },
  {
    path: 'clients',
    component: ClientManageComponent
  }
];

@NgModule({
  declarations: [
    AdminComponent,
    QuestionComponent,
    CandidateListComponent,
    CandidateDetailComponent,
    ImportQuestionComponent,
    EditSubjectComponent,
    SubjectDetailsComponent,
    TestResultComponent,
    EmailTrackComponent,
    TestGridComponent,
    TestQuestionAddComponent,
    SubjectAddComponent,
    DeletesubjectComponent,
    DeleteQuestionComponent,
    DeleteCandidateComponent,
    AddInstructionComponent,
    DeleteInstructionComponent,
    DeleteTestComponent,
    FinishModelComponent,
    QuestionAddComponent,
    TopicAddEditComponent,
    AddCandidateComponent,
    AdminManageComponent,
    AddAdminComponent,
    ImportCandidateListComponent,
    AdminProfileComponent,
    AdminPermissionsComponent,
    AddRoleComponent,
    CandidateGroupsComponent,
    AddGroupComponent,
    EditImportCandidateComponent,
    AddTestComponent,
    CandidateTestsComponent,
    TestQuestionsComponent,
    TestInstructionsComponent,
    ViewInstructionsComponent,
    EditMarksComponent,
    AnswerEvaluationComponent,
    ClientManageComponent,
    AddEditClientComponent,
    DeleteClientComponent,
    DeleteGroupComponent
  ],
  imports: [
    SharedModule,
    CommonModule,
    QuillModule.forRoot(),
    RouterModule.forChild(appRoutes),
       NgxMatSelectSearchModule,
  ],
  providers: [{ provide: Papa, useFactory: () => new Papa() }],

})
export class AdminModule { }
