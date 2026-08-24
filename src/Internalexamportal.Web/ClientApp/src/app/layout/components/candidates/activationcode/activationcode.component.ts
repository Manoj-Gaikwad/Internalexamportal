import { Component, OnInit } from '@angular/core';
import { ViewEncapsulation } from "@angular/core";
import { fuseAnimations } from "@fuse/animations";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { FuseConfigService } from "@fuse/services/config.service";
import { TestDataService } from 'app/layout/services/test-data-service.service';
import { CandidateService } from 'app/layout/services/candidate.service';
import { fuseFullScreenConfig } from 'app/layout/entities/globalConstants';
import { SnackBarService } from 'app/layout/services/snackbar.service';

@Component({
    selector: 'cg-activationcode',
    templateUrl: './activationcode.component.html',
    styleUrls: ['./activationcode.component.scss'],
    animations: fuseAnimations,
    standalone: false
})
export class ActivationcodeComponent implements OnInit {
  testId: any;
  InstructionId: any;
  public isSpinner: boolean = false;

  activationForm: FormGroup;
  testResult: any;
  ActivationId: any;
  data: any;
  Id: any;
  ActivationTestCode: any;
  constructor(private _candidateService: CandidateService,
    private _formBuilder: FormBuilder,
    private router: Router,
    private snackBar: SnackBarService,
    private testDataService: TestDataService,
    private _fuseConfigService: FuseConfigService) {
    this._fuseConfigService.config = fuseFullScreenConfig;
  }

  ngOnInit() {
    this.activationForm = this._formBuilder.group({
      activation: ['', [Validators.required]],
    });

    if (window.opener && window.opener !== window) {
      if (window.opener.origin != window.origin) {
        this.router.navigate(['./404']);
      }
    } else {
      this.router.navigate(['./404']);
    }

  }

  ActivateExam() {
    this.showSpinner();
    this.ActivationTestCode = this.activationForm.value.activation;
    this._candidateService.GetTestByActivation(this.ActivationTestCode)
      .subscribe((data: any) => {
        this.testResult = data;
        if (this.testResult != null) {

          if (this.testResult.success) {
            this.testDataService.testId = this.testResult.testDetails.id;
            this.testDataService.testName = this.testResult.testDetails.testName;
            this.testDataService.testDuration = this.testResult.testDetails.duration;
            this.testDataService.testTotalQuestion = this.testResult.testDetails.totalQuestion;
            this.testDataService.testTotalMark = this.testResult.testDetails.totalMark;
            this.testDataService.testTypeId = this.testResult.testDetails.testTypeId
            this.testDataService.testInstructionId = this.testResult.testDetails.testInstructionId;
            this.testDataService.difficultLevelId = this.testResult.testDetails.difficultLevelId;
            this.testDataService.testlinkId = this.testResult.testDetails.linkId;
            this.testDataService.setting = this.testResult.testSettings;
            this.router.navigate(['./layout/candidates/examrule']);
          } else {
            this.snackBar.error(this.testResult.message);

          }

        } else {

          this.snackBar.error('No Response from server');

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
