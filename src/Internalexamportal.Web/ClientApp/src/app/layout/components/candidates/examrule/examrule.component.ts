import { Component, OnInit, ElementRef, HostListener, Input, TemplateRef, ViewChild } from '@angular/core';
import { RouterModule, Routes, Router } from '@angular/router';
import { FuseConfigService } from "@fuse/services/config.service";
import { FusePerfectScrollbarDirective } from '@fuse/directives/fuse-perfect-scrollbar/fuse-perfect-scrollbar.directive';
import { FuseSidebarService } from '@fuse/components/sidebar/sidebar.service';
import { SharedService } from "app/layout/services/shared.service";
import { CustomHttpService } from "app/layout/services/custom-http.service";
import { HttpClient } from "@angular/common/http";
import { FormBuilder } from "@angular/forms";
import { ViewEncapsulation } from "@angular/core";
import { fuseAnimations } from "@fuse/animations";
import { HttpErrorResponse } from "@angular/common/http";
import { ActivatedRoute } from "@angular/router";
import { TestDataService } from 'app/layout/services/test-data-service.service';
import { CandidateService } from 'app/layout/services/candidate.service';
import { fuseFullScreenConfig } from 'app/layout/entities/globalConstants';
import { SnackBarService } from 'app/layout/services/snackbar.service';
import { MatDialog } from '@angular/material/dialog';

@Component({
    selector: 'cg-examrule',
    templateUrl: './examrule.component.html',
    styleUrls: ['./examrule.component.scss'],
    animations: fuseAnimations,
    standalone: false
})
export class ExamruleComponent implements OnInit {
  testId: any;
  Id: any;

  public isSpinner: boolean = false;

  oncontextmenu: false;
  instructionSet: string = '';
  plainText: any;
  public user: any;
  @ViewChild('continueDialog') continueDialog: TemplateRef<any>;

  constructor(private _candidateService: CandidateService,
    private _sharedService: SharedService,
    private router: Router,
    private testDataService: TestDataService,
    private _fuseConfigService: FuseConfigService,
    private _snackbar: SnackBarService,
    private dialog: MatDialog) {
    this._fuseConfigService.config = fuseFullScreenConfig;
  }

  ngOnInit() {
    if (this.testDataService.testId == 0) {
      this.router.navigate(['./layout/candidates/my-test']);
      return;
    }
    this.getInstructions();
    this.user = this._sharedService.getUserFromToken();
  }

  getInstructions() {
    this.showSpinner();
    this._candidateService.GetTestInstruction(this.testDataService.testInstructionId)
      .subscribe((data: any) => {
        this.instructionSet = data.instructionDescripiton;
        this.hideSpinner();
      },
        err => {
          console.log(err.message);
          this.hideSpinner();
        }
      );
  }

  examrule() {
    this.showSpinner();
    this._candidateService.StartTest({ testId: this.testDataService.testId })
      .subscribe((res: any) => {
        if (res.success) {
          this.testDataService.submittedTestId = res.submittedTestId;
          this.testDataService.isPendingTest = res.isPendingTest;
          if (res.isPendingTest) {
            this.testDataService.timeTaken = res.timeTaken;
            const dialogRef = this.dialog.open(this.continueDialog, {
              width: '400px',
              panelClass: ['app-no-padding-dialog'],
              disableClose: true,
              restoreFocus: false
            });
            dialogRef.afterClosed().subscribe(result => {
              this.router.navigated = true;
              this.router.navigate(['./layout/candidates/currentexam']);
            })
          }else{
            this.router.navigated = true;
            this.router.navigate(['./layout/candidates/currentexam']);
          }
        } else {
          this._snackbar.error(res.message);
        }
        this.hideSpinner();
      },
        err => {
          console.log(err.message);
          this.hideSpinner();
        }
      );

  }

  onRightClick() {
    return false;
  }

  showSpinner() { this.isSpinner = true }

  hideSpinner() { this.isSpinner = false }
}
