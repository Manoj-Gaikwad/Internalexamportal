import { Routes } from '@angular/router';

import { AuthenticationGuard } from './layout/security/auth-guard';
import { LoginAuthorizationGuard } from './layout/security/login-authorization-guard';
import { Error404Component } from './main/errors/404/error-404.component';

export const appRoutes: Routes = [
 
    {
        path: '',
        canActivate: [LoginAuthorizationGuard],
        loadChildren: () => import('./layout/components/authentication/authentication.module').then(m => m.AuthenticationModule)
    },
    {
        path: 'layout',
        canActivate: [AuthenticationGuard],
        loadChildren: () => import('./layout/authenticated-layout.module').then(m => m.AuthenticatedLayoutModule)
    },
    {
        path: '**',
        component: Error404Component
    }
];