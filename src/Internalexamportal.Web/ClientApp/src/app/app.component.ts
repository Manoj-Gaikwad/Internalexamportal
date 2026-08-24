import { Component, Inject, OnDestroy, OnInit, DOCUMENT } from '@angular/core';

import { Platform } from '@angular/cdk/platform';
import { TranslateService } from '@ngx-translate/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

import { FuseConfigService } from '@fuse/services/config.service';
import { FuseNavigationService } from '@fuse/components/navigation/navigation.service';
import { FuseSplashScreenService } from '@fuse/services/splash-screen.service';
import { FuseTranslationLoaderService } from '@fuse/services/translation-loader.service';

import { navigation } from 'app/navigation/navigation';
import { locale as navigationEnglish } from 'app/navigation/i18n/en';
import { locale as navigationTurkish } from 'app/navigation/i18n/tr';
import { HttpClient } from "@angular/common/http";

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss'],
    standalone: false
})
export class AppComponent implements OnInit, OnDestroy {
    fuseConfig: any;
    navigation: any;

    private _unsubscribeAll: Subject<void> = new Subject<void>();
    
    constructor(
        @Inject(DOCUMENT) private document: any,
        private _fuseConfigService: FuseConfigService,
        private _fuseSplashScreenService: FuseSplashScreenService,
        private _fuseTranslationLoaderService: FuseTranslationLoaderService,
        private _translateService: TranslateService,
        private _platform: Platform,
        private httpService: HttpClient,
        private _fuseNavigationService: FuseNavigationService   //  inject service
    ) {
        // Get default navigation
        this.navigation = navigation;

        //  Register the navigation with the service
        this._fuseNavigationService.register('main', this.navigation);

        //  Set it as the current navigation
        this._fuseNavigationService.setCurrentNavigation('main');

        // Translations
        this._translateService.addLangs(['en', 'tr']);
        this._translateService.setDefaultLang('en');
        this._fuseTranslationLoaderService.loadTranslations(navigationEnglish, navigationTurkish);
        this._translateService.use('en');

        // Mobile check
        if (this._platform.ANDROID || this._platform.IOS) {
            this.document.body.classList.add('is-mobile');
        }

        this._unsubscribeAll = new Subject();
    }

    ngOnInit(): void {
        this._fuseConfigService.config
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((config) => {
                this.fuseConfig = config;

                if (this.fuseConfig.layout.width === 'boxed') {
                    this.document.body.classList.add('boxed');
                } else {
                    this.document.body.classList.remove('boxed');
                }

                for (let i = 0; i < this.document.body.classList.length; i++) {
                    const className = this.document.body.classList[i];
                    if (className.startsWith('theme-')) {
                        this.document.body.classList.remove(className);
                    }
                }

                this.document.body.classList.add(this.fuseConfig.colorTheme);
            });
    }

    ngOnDestroy(): void {
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
    }
}
