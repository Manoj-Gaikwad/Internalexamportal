import { Component, OnInit, Inject } from "@angular/core";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { PrivilegeService } from "app/layout/services/privilege.service";
import { AdminService } from "app/layout/services/admin.service";
import { isNullOrUndefined } from "util";
import { SnackBarService } from 'app/layout/services/snackbar.service';

@Component({
    selector: "cg-add-group",
    templateUrl: "./add-group.component.html",
    styleUrls: ["./add-group.component.scss"],
    providers: [AdminService],
    standalone: false
})
export class AddGroupComponent implements OnInit {
    isSpinner: boolean = false;
    form: FormGroup;
    isEdit: boolean = false;
    id: number = 0;
    name: string = null;
    title: string = "Add Group";

    constructor(
        private _adminService: AdminService,
        private _formBuilder: FormBuilder,
        private dialogRef: MatDialogRef<any>,
        @Inject(MAT_DIALOG_DATA) public parentData: any,
        private snackBar: SnackBarService
    ) { }

    ngOnInit() {
        if (!isNullOrUndefined(this.parentData)) {
            this.id = this.parentData.id;
            this.name = this.parentData.name;
            this.isEdit = true;
            this.title = "Edit Group";
        }

        this.buildForm();
    }

    buildForm() {
        this.form = this._formBuilder.group({
            id: [this.id],
            name: [this.name, [Validators.required, Validators.maxLength(50)]],
        });
    }

    onSubmit() {
        var form = this.form.value;
        this.addGroup(form);
    }

    addGroup(form) {
        this.showSpinner();
        this._adminService.AddGroup(form).subscribe(
            (res: any) => {
                this.hideSpinner();
                if (res.success) {
                    if (this.isEdit) {
                        this.snackBar.success("Group Updated Successfully.");
                    } else {
                        this.snackBar.success("Group Added Successfully.");
                    }
                    this.dialogRef.close(true);
                } else {
                    this.snackBar.error(res.message);
                }
            },
            (err) => {
                console.error(err);
                this.hideSpinner();
            }
        );
    }

    showSpinner() {
        this.isSpinner = true;
    }
    hideSpinner() {
        this.isSpinner = false;
    }
}
