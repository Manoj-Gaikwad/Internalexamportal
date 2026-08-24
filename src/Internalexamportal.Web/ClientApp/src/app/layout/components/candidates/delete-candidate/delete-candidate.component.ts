import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { CustomHttpService } from "app/layout/services/custom-http.service";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { Inject } from "@angular/core";
import { SnackBarService } from 'app/layout/services/snackbar.service';


@Component({
    selector: 'cg-delete-candidate',
    templateUrl: './delete-candidate.component.html',
    styleUrls: ['./delete-candidate.component.scss'],
    standalone: false
})
export class DeleteCandidateComponent implements OnInit {
  Id: any;
  public errorMsg = [];

  constructor(private router: Router,
    private http: CustomHttpService,
    @Inject(MAT_DIALOG_DATA) public parentData: any,
    private dialogRef: MatDialogRef<any>,
    private snackBar: SnackBarService) { }

  ngOnInit() {
  }

  DeleteCandidate() {
    this.errorMsg = [];
    this.Id = this.parentData.CandidateId;
    this.http._delete('candidate/DeleteCandidate/' + this.Id)
      .subscribe(res => {
        this.snackBar.success('User Deleted Successfully');
        this.dialogRef.close(true);
      },
        err => {
          this.errorMsg = err.error[""][0];
        }
      );
  }
  CloseDialog() {
    this.dialogRef.close();
  }
}
