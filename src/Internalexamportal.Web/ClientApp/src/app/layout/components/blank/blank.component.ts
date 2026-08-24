import { Component } from '@angular/core';

import { fuseAnimations } from '@fuse/animations';
import { FusePerfectScrollbarDirective } from '@fuse/directives/fuse-perfect-scrollbar/fuse-perfect-scrollbar.directive';
import { FuseSidebarService } from '@fuse/components/sidebar/sidebar.service';

@Component({
    selector: 'blank',
    templateUrl: './blank.component.html',
    styleUrls: ['./blank.component.scss'],
    animations: fuseAnimations,
    standalone: false
})
export class BlankComponent
{
    /**
     * Constructor
     */
    constructor(
    )
    {
    }
}
