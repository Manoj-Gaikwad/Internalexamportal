import { NgModule, LOCALE_ID } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule, PreloadAllModules } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';

// Fuse imports
import { FuseModule } from '@fuse/fuse.module';
import { FuseSharedModule } from '@fuse/shared.module';
import { FuseProgressBarModule } from '@fuse/components';
import { fuseConfig } from 'app/fuse-config';

// App imports
import { AppComponent } from 'app/app.component';
import { LayoutModule } from 'app/layout/layout.module';
import { SampleModule } from 'app/main/sample/sample.module';
import { SharedModule } from './shared/shared.module';
import { AuthenticationModule } from './layout/components/authentication/authentication.module';
import { appRoutes } from './app.routing';

// Services
import { CustomHttpService } from './layout/services/custom-http.service';
import { SharedService } from './layout/services/shared.service';
import { AuthenticationGuard } from './layout/security/auth-guard';
import { LoginAuthorizationGuard } from './layout/security/login-authorization-guard';
import { RouteAuthorizationGuard } from './layout/security/route-authorization-guard';
import { RoleGuard } from './layout/security/role-guard';
import { TokenInterceptor } from './layout/services/token.interceptor';
import { DataService } from './layout/components/candidates/candidate.service';
import { TestDataService } from './layout/services/test-data-service.service';
import { RedirectService } from './layout/services/redirect-url.service';

// Directives & Components
import { NoRightClickDirective } from './layout/directives/no-right-click.directive';

// Angular Material Date Locale
import { MAT_DATE_LOCALE } from '@angular/material/core';


@NgModule({ declarations: [
        AppComponent,
        NoRightClickDirective,
    ],
    bootstrap: [AppComponent], imports: [BrowserModule,
        BrowserAnimationsModule,
        RouterModule.forRoot(appRoutes, {
            useHash: true,
            preloadingStrategy: PreloadAllModules
        }),
        TranslateModule.forRoot(),
        // Fuse modules
        FuseModule.forRoot(fuseConfig),
        FuseProgressBarModule,
        FuseSharedModule,
        // App modules
        LayoutModule,
        SampleModule,
        SharedModule,
        AuthenticationModule], providers: [
        CustomHttpService,
        SharedService,
        DataService,
        TestDataService,
        RedirectService,
        AuthenticationGuard,
        LoginAuthorizationGuard,
        RoleGuard,
        RouteAuthorizationGuard,
        {
            provide: HTTP_INTERCEPTORS,
            useClass: TokenInterceptor,
            multi: true
        },
        { provide: LOCALE_ID, useValue: 'en-IN' },
        { provide: MAT_DATE_LOCALE, useValue: 'en-IN' },
        provideHttpClient(withInterceptorsFromDi())
    ] })
export class AppModule {}
