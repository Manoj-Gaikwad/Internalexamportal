import { Component, OnInit } from '@angular/core';
import { CustomHttpService } from "app/layout/services/custom-http.service";
import { Router } from "@angular/router";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { Inject } from "@angular/core";
import { SnackBarService } from 'app/layout/services/snackbar.service';

@Component({
    selector: 'cg-delete-test',
    templateUrl: './delete-test.component.html',
    styleUrls: ['./delete-test.component.scss'],
    standalone: false
})
export class DeleteTestComponent implements OnInit {
Id: any;
  errorMsg: any;

  constructor(private router: Router,
              private http: CustomHttpService,
              @Inject(MAT_DIALOG_DATA) public parentData: any,
              private dialogRef: MatDialogRef<any>,
              private snackBar: SnackBarService) { }

  ngOnInit() {
  }
  deleterow(){
   this.Id=this.parentData.TestId;
   this.http._delete('Test/DeleteTest/' + this.Id)
      .subscribe(
      (res: any) => {
           this.snackBar.success('Test deleted Successfully');
            this.dialogRef.close(true);
      },
      err => {
        this.errorMsg = err.error[""][0];
      }
      );
}

}
