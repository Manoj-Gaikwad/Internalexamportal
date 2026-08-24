import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FuseConfigService } from '@fuse/services/config.service';
import { fuseFullScreenConfig } from 'app/layout/entities/globalConstants';
import { TestDataService } from 'app/layout/services/test-data-service.service';

@Component({
    selector: 'cg-submit-success',
    templateUrl: './submit-success.component.html',
    styleUrls: ['./submit-success.component.scss'],
    standalone: false
})
export class SubmitSuccessComponent implements OnInit {

  constructor(private router: Router,
    private _fuseConfigService: FuseConfigService,
    private dataService: TestDataService) {
    this._fuseConfigService.config = fuseFullScreenConfig;
  }

  ngOnInit() {
    if (this.dataService.testId == 0) {
      this.router.navigate(['./404']);
    }
  }


  closeWindow() {
    if (window.opener && window.opener !== window) {
      window.close();
    } else {
      this.router.navigate(['./layout/candidates/my-test']);
    }
  }
  
  onRightClick() {
    return false;
  }
}
