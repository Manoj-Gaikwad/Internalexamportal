import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FuseConfigService } from '@fuse/services/config.service';
import { Router, ActivatedRoute } from '@angular/router';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MAT_DATE_FORMATS } from '@angular/material/core';
import { AdminService } from 'app/layout/services/admin.service';
import { isNullOrUndefined } from 'util';
import { DATE_FORMAT, emailPattern, inputNamePattern, phoneNumberPattern, Roles, minBirthDate } from 'app/layout/entities/globalConstants';
import { SnackBarService } from 'app/layout/services/snackbar.service';
import { ClientService } from 'app/layout/services/client.service';
import { SharedService } from 'app/layout/services/shared.service';

@Component({
    selector: 'cg-add-admin',
    templateUrl: './add-admin.component.html',
    styleUrls: ['./add-admin.component.scss'],
    providers: [AdminService, ClientService],
    standalone: false
})
export class AddAdminComponent implements OnInit {

  public title: string = "Add User";
  addUserForm: FormGroup;
  public roles: Array<any>;
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
    this.getRoles();

    this.isSuperAdmin = this._sharedService.checkForRole(Roles.SuperAdmin);
    if (this.isSuperAdmin) {
      this.getClients();
    }

    if (!isNullOrUndefined(this.parentData)) {
      this.Id = this.parentData.id;
      this.getUser();
      this.isEdit = true;
      this.title = "Edit User";
    }
    this.initializeForm();

  }

  initializeForm() {
    this.addUserForm = this._formBuilder.group({
      Id: [this.Id],
      email: ['', [Validators.required, Validators.maxLength(50), Validators.pattern(emailPattern)]],
      firstname: ['', [Validators.required, Validators.pattern(inputNamePattern), Validators.maxLength(20)]],
      lastname: ['', [Validators.required, Validators.pattern(inputNamePattern), Validators.maxLength(20)]],
      username: [''],
      gender: ['', [Validators.required]],
      dob: [''],
      phonenumber: ['', [Validators.required, Validators.pattern(phoneNumberPattern), Validators.minLength(10), Validators.maxLength(10)]],
      role: [null, Validators.required],
      clientId: [],
      isAdmin: [true]
    });
  }

  getRoles() {
    this.showSpinner()
    this.adminService.getRoles()
      .subscribe((res: any) => {
        this.roles = res.roles;
        this.hideSpinner();
      }, err => {
        this.hideSpinner();
        this.errorMsg = err.error[""][0];
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
  
  getUser() {
    this.showSpinner()
    this.adminService.getUser(this.Id)
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
    this.addUserForm.patchValue({
      email: this.candidateData.email,
      username: this.candidateData.userName,
      firstname: this.candidateData.firstName,
      lastname: this.candidateData.lastName,
      phonenumber: this.candidateData.phoneNumber,
      gender: this.candidateData.gender,
      dob: this.candidateData.dateOfBirth,
      role: this.candidateData.role,
      clientId: this.candidateData.clientId
    });
    this.addUserForm.markAsTouched();
    this.markFormGroupTouched(this.addUserForm);
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
    this.adminService.UpdateUser(this.addUserForm.value)
      .subscribe(res => {
        if (res.success) {
          this.snackBar.success('User Information Updated Successfully');
          this.dialogRef.close(true);
        } else {
          this.snackBar.error(res.message);
        }
      },
        err => {
          this.snackBar.error(err.error[""][0]);
          this.errorMsg = err.error[""][0];
          this.hideSpinner();
        }, () => {
          this.hideSpinner();
        }
      );
  }

  AddCandidate() {
    this.showSpinner();
    this.adminService.AddUser(this.addUserForm.value)
      .subscribe(res => {
        this.snackBar.success('User Created Successfully. Check email for login credentials.');
        this.dialogRef.close(true);
      }, err => {
        this.snackBar.error(err.error[""][0]);
        this.errorMsg = err.error[""][0];
        this.hideSpinner();
      }, () => {
        this.hideSpinner();
      }
      );
  }

  showSpinner() { this.isSpinner = true }

  hideSpinner() { this.isSpinner = false }
}
