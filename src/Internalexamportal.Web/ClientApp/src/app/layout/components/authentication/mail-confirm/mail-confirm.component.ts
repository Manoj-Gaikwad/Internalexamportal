import { Component, ViewEncapsulation, OnInit } from '@angular/core';

import { FuseConfigService } from '@fuse/services/config.service';
import { fuseAnimations } from '@fuse/animations';
import { ActivatedRoute, Router } from '@angular/router';
import { CustomHttpService } from 'app/layout/services/custom-http.service';

@Component({
    selector: 'mail-confirm',
    templateUrl: './mail-confirm.component.html',
    styleUrls: ['./mail-confirm.component.scss'],
    encapsulation: ViewEncapsulation.None,
    animations: fuseAnimations,
    standalone: false
})
export class MailConfirmComponent implements OnInit {
    /**
     * Constructor
     *
     * @param {FuseConfigService} _fuseConfigService
     */
    constructor(private route: ActivatedRoute,
        private http: CustomHttpService,
        private router: Router,
        private _fuseConfigService: FuseConfigService
    ) {
        // Configure the layout
        this._fuseConfigService.config = {
            layout: {
                navbar: {
                    hidden: true
                },
                toolbar: {
                    hidden: true
                },
                footer: {
                    hidden: true
                },
                sidepanel: {
                    hidden: true
                }
            }
        };
    }
    public isWorking: boolean = true;

    ngOnInit() {
        localStorage.clear();
        // received UserId and EmailConfirmationCode from the url. We need to send this back to the api.
        const UserId = this.route.snapshot.queryParams['UserId'];
        const EmailConfirmationCode = encodeURIComponent(this.route.snapshot.queryParams['EmailConfirmationCode']);
        this.isWorking = true;

        // Send request to confirmEmail end point and subscribe the response.
        const httpRequestBody = { userId: UserId, emailConfirmationCode: EmailConfirmationCode };
        this.http._post('account/confirmEmail?', httpRequestBody)
            .subscribe(
                (res: any) => {
                    this.isWorking = false;
                    // If email confirmation was successfull then save the treceived token and log the user in. 
                    if (res.succeeded === true) {

                    } else {

                    }
                },
                onerror => {
                    this.isWorking = false;

                }
            );
    }
}
