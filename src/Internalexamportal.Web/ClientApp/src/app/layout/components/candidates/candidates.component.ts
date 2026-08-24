import { Component, OnInit } from '@angular/core';
import { fuseAnimations } from '@fuse/animations';
import { FusePerfectScrollbarDirective } from '@fuse/directives/fuse-perfect-scrollbar/fuse-perfect-scrollbar.directive';
import { FuseSidebarService } from '@fuse/components/sidebar/sidebar.service';

@Component({
    selector: 'cg-candidates',
    templateUrl: './candidates.component.html',
    styleUrls: ['./candidates.component.scss'],
    standalone: false
})
export class CandidatesComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}
