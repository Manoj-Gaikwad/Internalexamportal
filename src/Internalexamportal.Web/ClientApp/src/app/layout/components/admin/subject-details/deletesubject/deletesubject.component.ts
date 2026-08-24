import { Component, OnInit } from '@angular/core';
import { CustomHttpService } from "app/layout/services/custom-http.service";
import { Router } from "@angular/router";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { Inject } from "@angular/core";
import { SnackBarService } from 'app/layout/services/snackbar.service';

@Component({
    selector: 'cg-deletesubject',
    templateUrl: './deletesubject.component.html',
    styleUrls: ['./deletesubject.component.scss'],
    standalone: false
})
export class DeletesubjectComponent implements OnInit {
  Id: any;
  errorMsg: any;

  constructor(private router: Router,
              private http: CustomHttpService,
              @Inject(MAT_DIALOG_DATA) public parentData: any,
              private snackBar: SnackBarService,
              private dialogRef: MatDialogRef<any>) { }

  ngOnInit() {

  }
deleterow(){
   this.Id=this.parentData.SubjectId;
   this.http._delete('question/DeleteSubject/' + this.Id)
      .subscribe(
      (res: any) => {
           this.snackBar.success('Subject deleted Successfully');
            this.dialogRef.close(true);
      },
      err => {
        this.errorMsg = err.error[""][0];
      }
      );
}

}
