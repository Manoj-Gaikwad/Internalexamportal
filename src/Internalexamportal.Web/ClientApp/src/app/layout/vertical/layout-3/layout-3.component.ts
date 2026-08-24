import { Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

import { FuseConfigService } from '@fuse/services/config.service';
import { navigation } from 'app/navigation/navigation';
import { AfterViewInit } from "@angular/core";

@Component({
    selector: 'vertical-layout-3',
    templateUrl: './layout-3.component.html',
    styleUrls: ['./layout-3.component.scss'],
    encapsulation: ViewEncapsulation.None,
    standalone: false
})
export class VerticalLayout3Component implements OnInit, OnDestroy {
    fuseConfig: any;
    navigation: any;
    currentExam : any;

    // Private
        private _unsubscribeAll: Subject<void> = new Subject<void>();

    /**
     * Constructor
     *
     * @param {FuseConfigService} _fuseConfigService
     */
    constructor(
        private _fuseConfigService: FuseConfigService
    ) {
        // Set the defaults
        this.navigation = navigation;

        // Set the private defaults
        this._unsubscribeAll = new Subject();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------
    // ngAfterViewInit() {
    //     this._fuseConfigService.config
    //         .pipe(takeUntil(this._unsubscribeAll))
    //         .subscribe((config) => {
    //             this.fuseConfig = config;
    //         });
    // }   
     /**
     * On init
     */
    ngOnInit(): void {
        // Subscribe to config changes
        this._fuseConfigService.config
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((config) => {
                this.fuseConfig = config;
            });
    }

    /**
     * On destroy
     */
    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next(undefined);
        this._unsubscribeAll.complete();
    }
}
