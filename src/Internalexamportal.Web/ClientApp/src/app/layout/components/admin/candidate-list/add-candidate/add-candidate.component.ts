import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FuseConfigService } from '@fuse/services/config.service';
import { Router, ActivatedRoute } from '@angular/router';
import { MatDialog, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AdminService } from 'app/layout/services/admin.service';
import { isNullOrUndefined } from 'util';
import { inputNamePattern, numberPattern, emailPattern, phoneNumberPattern, Roles, minBirthDate } from 'app/layout/entities/globalConstants';
import { SnackBarService } from 'app/layout/services/snackbar.service';
import { SharedService } from 'app/layout/services/shared.service';
import { ClientService } from 'app/layout/services/client.service';

@Component({
    selector: 'cg-add-candidate',
    templateUrl: './add-candidate.component.html',
    styleUrls: ['./add-candidate.component.scss'],
    providers: [AdminService, ClientService],
    standalone: false
})
export class AddCandidateComponent implements OnInit {

  public title: string = "Add Candidate";
  candidateDetailForm: FormGroup;
  public clients: Array<any>;
  public isSuperAdmin: boolean = false;
  isSpinner: boolean = false;
  Id: any = null;
  isEdit: boolean = false;
  public errorMsg = [];
  candidateData: any;
  candidateUpdateData: any;
  public maxDate: Date;
  public minDate:Date = minBirthDate;

  public groups: Array<any>;

  constructor(@Inject(MAT_DIALOG_DATA) public parentData: any,
    private dialogRef: MatDialogRef<any>,
    private _formBuilder: FormBuilder,
    private router: Router,
    public dialog: MatDialog,
    private snackBar: SnackBarService,
    private adminService: AdminService,
    private _clientService: ClientService,
    private _sharedService: SharedService) {
  }

  ngOnInit() {

    this.maxDate = new Date();
    this.getAllGroups();
    
    this.isSuperAdmin = this._sharedService.checkForRole(Roles.SuperAdmin);
    if (this.isSuperAdmin) {
      this.getClients();
    }
    
    if (!isNullOrUndefined(this.parentData)) {
      this.Id = this.parentData.id;
      this.GetCandidateDetailsById();
      this.isEdit = true;
      this.title = "Edit Candidate";
    }
    this.initializeForm();

  }

  initializeForm() {
    this.candidateDetailForm = this._formBuilder.group({
      Id: [this.Id],
      email: ['', [Validators.required, Validators.maxLength(50), Validators.pattern(emailPattern)]],
      firstname: ['', [Validators.required, Validators.pattern(inputNamePattern), Validators.maxLength(20)]],
      lastname: ['', [ Validators.pattern(inputNamePattern), Validators.maxLength(20)]],
      username: [''],
      gender: ['', [Validators.required]],
      dob: [null],
      phonenumber: ['', [Validators.pattern(phoneNumberPattern), Validators.minLength(10), Validators.maxLength(10)]],
      group: [null, [Validators.required]],
      clientId: [0,[]]
    });
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

  getClients() {
    this.showSpinner()
    this._clientService.GetClients()
      .subscribe((res: any) => {
        this.clients = res.clients;
        this.hideSpinner();
      }, err => {
        this.hideSpinner();
        this.errorMsg = err.error[""][0];
      }
      );
  }
  
  GetCandidateDetailsById() {
    this.showSpinner()
    this.adminService.GetCandidateDetailsById(this.Id)
      .subscribe((res: any) => {
        this.candidateData = res;
        this.setFormValue();
        this.hideSpinner();
      },
        err => {
          this.errorMsg = err.error[""][0];
        }
      );
  }

  setFormValue() {

    this.candidateDetailForm.patchValue({
      email: this.candidateData.email,
      username: this.candidateData.userName,
      firstname: this.candidateData.firstName,
      lastname: this.candidateData.lastName,
      phonenumber: this.candidateData.phoneNumber,
      gender: this.candidateData.gender,
      dob: this.candidateData.dateOfBirth,
      group: this.candidateData.group,
      clientId: this.candidateData.clientId
    });
    this.candidateDetailForm.markAsTouched();
    this.markFormGroupTouched(this.candidateDetailForm)
  }

  private markFormGroupTouched(formGroup: FormGroup) {
    (<any>Object).values(formGroup.controls).forEach(control => {
      control.markAsTouched();

      if (control.controls) {
        this.markFormGroupTouched(control);
      }
    });
  }

  onSubmit() {
    if (this.isEdit) {
      this.updateCandidate();
    } else {
      this.AddCandidate();
    }
  }

  updateCandidate() {
    this.showSpinner();
    this.adminService.UpdateCandidateDetails(this.candidateDetailForm.value)
      .subscribe(res => {
        if (res.success) {
          this.snackBar.success('Candidate Information Updated Successfully');
          this.dialogRef.close(true);
        } else {
          this.snackBar.error(res.message);
        }
      },
        err => {
          this.hideSpinner();
          this.snackBar.error(err.error[""][0]);
          this.errorMsg = err.error[""][0];
        }, () => {
          this.hideSpinner();
        }
      );
  }

  AddCandidate() {
    this.showSpinner();
    this.adminService.AddCandidate(this.candidateDetailForm.value)
      .subscribe(res => {
        this.snackBar.success('Candidate Added Successfully. Check email for login credentials.');
        this.dialogRef.close(true);
      }, err => {
        this.hideSpinner();
        this.snackBar.error(err.error[""][0]);
        this.errorMsg = err.error[""][0];
      }, () => {
        this.hideSpinner();
      }
      );
  }

  showSpinner() { this.isSpinner = true }

  hideSpinner() { this.isSpinner = false }
}
