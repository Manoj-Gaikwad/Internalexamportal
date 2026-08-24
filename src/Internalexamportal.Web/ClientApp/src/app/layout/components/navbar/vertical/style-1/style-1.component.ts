import { Component, OnDestroy, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { delay, filter, take, takeUntil } from 'rxjs/operators';

import { FuseConfigService } from '@fuse/services/config.service';
import { FuseNavigationService } from '@fuse/components/navigation/navigation.service';
import { FusePerfectScrollbarDirective } from '@fuse/directives/fuse-perfect-scrollbar/fuse-perfect-scrollbar.directive';
import { FuseSidebarService } from '@fuse/components/sidebar/sidebar.service';
import { SharedService } from 'app/layout/services/shared.service';
import { navigation, normalUserNavigation } from 'app/navigation/navigation';
import { UserDataService } from 'app/layout/services/user.service';
import { Roles } from 'app/layout/entities/globalConstants';

@Component({
    selector: 'navbar-vertical-style-1',
    templateUrl: './style-1.component.html',
    styleUrls: ['./style-1.component.scss'],
    encapsulation: ViewEncapsulation.None,
    standalone: false
})
export class NavbarVerticalStyle1Component implements OnInit, OnDestroy {
    fuseConfig: any;
    navigation: any;
    public user: any = {};
    public userId: string;
    // Private
    private _fusePerfectScrollbar: FusePerfectScrollbarDirective;
     private _unsubscribeAll: Subject<void> = new Subject<void>();

    /**
     * Constructor
     *
     * @param {FuseConfigService} _fuseConfigService
     * @param {FuseNavigationService} _fuseNavigationService
     * @param {FuseSidebarService} _fuseSidebarService
     * @param {Router} _router
     */
    constructor(
        private _fuseConfigService: FuseConfigService,
        private _fuseNavigationService: FuseNavigationService,
        private _fuseSidebarService: FuseSidebarService,
        private _router: Router,
        private sharedService: SharedService,
        private _userDataService: UserDataService
    ) {
        // Set the private defaults
        this._unsubscribeAll = new Subject();
        if (localStorage.length > 0) {
            this.userId = this.sharedService.decodeTokenToGetUserId();
            this.getUserDetails();
            this._userDataService.getUserDetails();
        }

        if (this._fuseNavigationService.getCurrentNavigation()) {
            this._fuseNavigationService.unregister('main');
        }

        if (this.sharedService.checkForRole(Roles.Candidate)) {
            this._fuseNavigationService.register('main', normalUserNavigation);
            this._fuseNavigationService.setCurrentNavigation('main');
        } else {
            this._fuseNavigationService.register('main', navigation);
            this._fuseNavigationService.setCurrentNavigation('main');
            this.sharedService.isAuthorizedNavigationItems();
        }
    }


    getUserDetails() {
        this._userDataService.user
            .subscribe(user => {
                this.user = user;
            });
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    // Directive
    @ViewChild(FusePerfectScrollbarDirective)
    set directive(theDirective: FusePerfectScrollbarDirective) {
        if (!theDirective) {
            return;
        }

        this._fusePerfectScrollbar = theDirective;

        // Update the scrollbar on collapsable item toggle
        this._fuseNavigationService.onItemCollapseToggled
            .pipe(
                delay(500),
                takeUntil(this._unsubscribeAll)
            )
            .subscribe(() => {
                this._fusePerfectScrollbar.update();
            });

        // Scroll to the active item position
        this._router.events
            .pipe(
                filter((event) => event instanceof NavigationEnd),
                take(1)
            )
            .subscribe(() => {
              setTimeout(() => {
    const activeNavItem = document.querySelector('navbar .nav-link.active') as HTMLElement | null;

    if (activeNavItem && activeNavItem.offsetParent) {
        const activeItemOffsetTop = activeNavItem.offsetTop;
        const activeItemOffsetParentTop = (activeNavItem.offsetParent as HTMLElement).offsetTop;
        const scrollDistance = activeItemOffsetTop - activeItemOffsetParentTop - (48 * 3) - 168;

        if (this._fusePerfectScrollbar) {
            this._fusePerfectScrollbar.scrollToTop(scrollDistance);
        }
    }
}, 0);

            }
            );
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void {
        this._router.events
            .pipe(
                filter((event) => event instanceof NavigationEnd),
                takeUntil(this._unsubscribeAll)
            )
            .subscribe(() => {
                if (this._fuseSidebarService.getSidebar('navbar')) {
                    this._fuseSidebarService.getSidebar('navbar').close();
                }
            }
            );

        // Subscribe to the config changes
        this._fuseConfigService.config
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((config) => {
                this.fuseConfig = config;
            });

        // Get current navigation
        this._fuseNavigationService.onNavigationChanged
            .pipe(
                filter(value => value !== null),
                takeUntil(this._unsubscribeAll)
            )
            .subscribe(() => {
                this.navigation = this._fuseNavigationService.getCurrentNavigation();
            });
    }

    /**
     * On destroy
     */
    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Toggle sidebar opened status
     */
    toggleSidebarOpened(): void {
        this._fuseSidebarService.getSidebar('navbar').toggleOpen();
    }

    /**
     * Toggle sidebar folded status
     */
    toggleSidebarFolded(): void {
        this._fuseSidebarService.getSidebar('navbar').toggleFold();
    }
}
