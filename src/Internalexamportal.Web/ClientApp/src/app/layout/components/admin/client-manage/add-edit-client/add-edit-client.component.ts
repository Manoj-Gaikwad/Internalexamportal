import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FuseConfigService } from '@fuse/services/config.service';
import { Router, ActivatedRoute } from '@angular/router';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MAT_DATE_FORMATS } from '@angular/material/core';
import { AdminService } from 'app/layout/services/admin.service';
import { isNullOrUndefined } from 'util';
import { DATE_FORMAT, emailPattern, inputNamePattern, phoneNumberPattern } from 'app/layout/entities/globalConstants';
import { SnackBarService } from 'app/layout/services/snackbar.service';
import { ClientService } from 'app/layout/services/client.service';
import { Client } from 'app/layout/entities/models';

@Component({
    selector: 'add-edit-client',
    templateUrl: './add-edit-client.component.html',
    styleUrls: ['./add-edit-client.component.scss'],
    providers: [ClientService],
    standalone: false
})
export class AddEditClientComponent implements OnInit {

  public title: string = "Add Client";
  form: FormGroup;
  isSpinner: boolean = false;
  Id: number = null;
  isEdit: boolean = false;
  public errorMsg = [];
  candidateData: Client;
  public maxDate: Date;

  constructor(@Inject(MAT_DIALOG_DATA) public parentData: any,
    private dialogRef: MatDialogRef<any>,
    private _formBuilder: FormBuilder,
    private router: Router,
    public dialog: MatDialog,
    private snackBar: SnackBarService,
    private _clientService: ClientService) {
  }

  ngOnInit() {

    if (!isNullOrUndefined(this.parentData)) {
      this.Id = this.parentData.id;
      this.getUser();
      this.isEdit = true;
      this.title = "Edit Client";
    }
    this.initializeForm();

  }

  initializeForm() {
    this.form = this._formBuilder.group({
      id: [0],
      email: ['', [Validators.required, Validators.maxLength(50), Validators.pattern(emailPattern)]],
      name: ['', [Validators.required, Validators.pattern(inputNamePattern), Validators.maxLength(50)]],
      address: ['', [Validators.required]],
      logo: [''],
      phone: ['', [Validators.required, Validators.pattern(phoneNumberPattern), Validators.minLength(10), Validators.maxLength(10)]],
    });
  }

  getUser() {
    this.showSpinner()
    this._clientService.GetClient(this.Id)
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
    this.form.patchValue(this.candidateData);
    this.form.markAsTouched();
    this.markFormGroupTouched(this.form);
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
    this.showSpinner();
    this._clientService.AddEdit(this.form.value)
      .subscribe(res => {
        if (res.success) {
          this.snackBar.success('Client Updated Successfully');
          this.dialogRef.close(true);
        } else {
          this.snackBar.error(res.message);
        }
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
