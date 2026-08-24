import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login/login.component';
import { TwoFactorAuthenticationComponent } from './two-factor-authentication/two-factor-authentication.component';
import { SharedModule } from 'app/shared/shared.module';
import { RegisterComponent } from './register/register.component';
import { TermsAndConditionsComponent } from './terms-and-conditions/terms-and-conditions.component';
import { MailConfirmComponent } from './mail-confirm/mail-confirm.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { Routes, RouterModule } from '@angular/router';
import { LoginAuthorizationGuard } from 'app/layout/security/login-authorization-guard';
import { AuthenticationBootstrapComponent } from './authentication-bootstrap/authentication-bootstrap.component';
import { LayoutModule } from 'app/layout/layout.module';
import { CandidateLoginComponent } from 'app/layout/components/authentication/candidate-login/candidate-login.component';
import { AccountService } from 'app/layout/services/account.service';


const appRoutes: Routes = [
  {
    path: '',
    component: AuthenticationBootstrapComponent,
    children: [
      {
        path: '',
        component: CandidateLoginComponent,
         canActivate: [LoginAuthorizationGuard],
       
      },
      {
        path: 'admin',
        component: LoginComponent,
        canActivate: [LoginAuthorizationGuard],
      },
      {
        path: 'register',
        component: RegisterComponent
      },
      {
        path: 'confirm-email',
        component: MailConfirmComponent
      },
      {
        path: 'forgot-password',
        component: ForgotPasswordComponent
      },
      {
        path: 'two-factor-authentication',
        component: TwoFactorAuthenticationComponent
      },
      {
        path: 'reset-password',
        component: ResetPasswordComponent
      }
    ]
  },

]
@NgModule({
  declarations: [
    LoginComponent,
    TwoFactorAuthenticationComponent,
    RegisterComponent,
    TermsAndConditionsComponent,
    MailConfirmComponent,
    ForgotPasswordComponent,
    ResetPasswordComponent,
    AuthenticationBootstrapComponent,
    CandidateLoginComponent
    

  ],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild(appRoutes),
    LayoutModule,
  ],
  providers:[AccountService],


})
export class AuthenticationModule { }
