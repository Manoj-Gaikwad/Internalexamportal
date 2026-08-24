import { Component, OnInit, Input } from '@angular/core';
import { FormGroup } from "@angular/forms";
import { FormBuilder } from "@angular/forms";
import { Validators } from "@angular/forms";
import { DataServicesService } from "app/layout/services/data-services.service";
import { SnackBarService } from 'app/layout/services/snackbar.service';
import { TestService } from 'app/layout/services/test.service';
import { emailPattern } from 'app/layout/entities/globalConstants';
import { AdminService } from 'app/layout/services/admin.service';

@Component({
    selector: 'cg-email-track',
    templateUrl: './email-track.component.html',
    styleUrls: ['./email-track.component.scss'],
    providers: [TestService, AdminService],
    standalone: false
})
export class EmailTrackComponent implements OnInit {

  @Input() testId: any;
  @Input() activationType:number;
  errorMsg: any;
  emailForm: FormGroup;
  isSpinner: boolean;
  public groups: Array<any>;


  constructor(private _formBuilder: FormBuilder, private _testService: TestService
    , private adminService: AdminService, private snackBar: SnackBarService) { }

  ngOnInit() {
    this.getAllGroups();
    this.emailForm = this._formBuilder.group({
      TestId: this.testId,
      email: ['', [Validators.pattern(emailPattern)]],
      group:[null]
    })
  }


  getAllGroups() {
    this.showSpinner()
    this.adminService.getAllGroups()
      .subscribe((res: any) => {
        this.groups = res.groups;
        this.hideSpinner();
      },
        err => {
          this.hideSpinner();
          console.error(err);
        }
      );
  }

  onSubmit() {
    this.showSpinner()
    var emailForm = this.emailForm.value;
    emailForm.examUrl = document.baseURI + '#/layout/candidates/my-test';
    this._testService.SendMail(this.emailForm.value).subscribe(
      (res: any) => {
        if (res.success) {
          this.snackBar.success('Activation Code sent to email successfully');
          this.hideSpinner();
        } else {
          this.snackBar.error(res.message);
          this.hideSpinner();
        }
      },
      err => {
        this.hideSpinner();
        this.errorMsg = err.error[""][0];
      }
    );

  }

  showSpinner() { this.isSpinner = true }

  hideSpinner() { this.isSpinner = false }

}
