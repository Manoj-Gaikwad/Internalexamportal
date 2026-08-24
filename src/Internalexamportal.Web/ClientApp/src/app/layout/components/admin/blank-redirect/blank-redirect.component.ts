import { Component, OnInit } from '@angular/core';
import { SharedService } from 'app/layout/services/shared.service';
import { Router } from '@angular/router';
import { Roles } from 'app/layout/entities/globalConstants';
import { FuseConfigService } from '@fuse/services/config.service';
import { fuseConfig } from 'app/fuse-config';

@Component({
    selector: 'cg-blank-redirect',
    templateUrl: './blank-redirect.component.html',
    styleUrls: ['./blank-redirect.component.scss'],
    standalone: false
})
export class BlankRedirectComponent implements OnInit {

  constructor(private sharedService: SharedService, private router: Router,
    private _fuseConfigService: FuseConfigService) {
      this._fuseConfigService.config = fuseConfig;
     }

 ngOnInit() {
  // Only redirect if currently on /layout root
  if (this.router.url === '/layout' || this.router.url === '/layout/') {
    if (this.sharedService.checkForRole(Roles.Candidate)) {
      this.router.navigate(['/layout/candidates/my-test']);
    } else {
      this.router.navigate(['/layout/admin/dashboard']);
    }
  }
}

}
