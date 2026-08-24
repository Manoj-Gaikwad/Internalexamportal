import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LayoutBootstrapComponent } from './components/layout-bootstrap/layout-bootstrap.component';
import { BlankComponent } from './components/blank/blank.component';
import { AuthenticationGuard } from './security/auth-guard';
import { SharedModule } from 'app/shared/shared.module';
import { RouterModule, Routes } from '@angular/router';
import { LayoutModule } from 'app/layout/layout.module';
import { ViewTestResultComponent } from './components/admin/view-result/view-result.component';
import { RouteAuthorizationGuard } from './security/route-authorization-guard';
import { BlankRedirectComponent } from './components/admin/blank-redirect/blank-redirect.component';

import { ChangePasswordComponent } from './dialogs/change-password/change-password.component';
import { Roles } from './entities/globalConstants';
import { PrintDailogComponent } from './dialogs/print-dailog/print-dailog.component';

const appRoutes: Routes = [
  {
    path: '',
    component: LayoutBootstrapComponent,
    children: [
      {
        path: '',
        component: BlankRedirectComponent
      },
      {
        path: 'admin',
        canActivate: [AuthenticationGuard,RouteAuthorizationGuard],
        loadChildren: () => import('./components/admin/admin.module').then(m => m.AdminModule),
        data: {
          role: Roles.SuperAdmin
        }
      },
      {
        path: 'candidates',
        canActivate: [AuthenticationGuard,RouteAuthorizationGuard],
        loadChildren: () => import('./components/candidates/candidates.module').then(m => m.CandidatesModule),
        data: {
          role: Roles.Candidate
        }
      },
    ]
  },
]

@NgModule({
  declarations: [
    LayoutBootstrapComponent,
    BlankComponent,
    ViewTestResultComponent,
    BlankRedirectComponent,
    ChangePasswordComponent,
    PrintDailogComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild(appRoutes),
    LayoutModule,
  
  ],
  exports:[BlankRedirectComponent,BlankComponent],
 



})
export class AuthenticatedLayoutModule { }
