import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { QuestionService } from 'app/layout/services/question.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { PrivilegeService } from 'app/layout/services/privilege.service';
import { isNullOrUndefined } from 'util';
import { SnackBarService } from 'app/layout/services/snackbar.service';

@Component({
    selector: 'add-role',
    templateUrl: './add-role.component.html',
    styleUrls: ['./add-role.component.scss'],
    providers: [PrivilegeService],
    standalone: false
})
export class AddRoleComponent implements OnInit {

  isSpinner: boolean = false;
  form: FormGroup;
  roleId: string = '';
  name: string = '';
  isEdit: boolean = false;
  title: string = 'Add Role';

  constructor(private _privilegeService: PrivilegeService, private _formBuilder: FormBuilder,
    private dialogRef: MatDialogRef<any>, @Inject(MAT_DIALOG_DATA) public parentData: any,
    private snackBar: SnackBarService) { }

  ngOnInit() {
    if (!isNullOrUndefined(this.parentData)) {
      this.roleId = this.parentData.id;
      this.name = this.parentData.name;
      this.isEdit = true;
      this.title = "Edit Role";
    }
    this.buildForm();
  }

  buildForm() {
    this.form = this._formBuilder.group({
      id: [this.roleId],
      name: [this.name, Validators.required]
    });
  }

  onSubmit() {
    var form = this.form.value;
    if (this.isEdit) {
      this.updateRole(form);
    } else {
      this.addRole(form)
    }
  }

  addRole(form) {
    this.showSpinner();
    this._privilegeService.AddRole(form)
      .subscribe(
        (res: any) => {
          if (res.succeeded) {
            this.snackBar.success('Role Added Successfully.');
            this.dialogRef.close(true);
          } else {
            this.snackBar.error('Role Already Present.');
          }
          this.hideSpinner();
        },
        err => {
          console.error(err);
          this.hideSpinner();
        }
      );
  }

  updateRole(form) {
    this.showSpinner();
    this._privilegeService.updateRole(form)
      .subscribe(
        (res: any) => {
          if (res.succeeded) {
            this.snackBar.success('Role Updated Successfully.');
            this.dialogRef.close(true);
          } else {
            this.snackBar.error('Role With Same Name Already Present.');
          }
          this.hideSpinner();
        },
        err => {
          console.error(err);
          this.hideSpinner();

        }
      );
  }

  showSpinner() { this.isSpinner = true }
  hideSpinner() { this.isSpinner = false }
}
