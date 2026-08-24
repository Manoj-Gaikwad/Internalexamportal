import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { CustomHttpService } from "app/layout/services/custom-http.service";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { Inject } from "@angular/core";
import { HttpErrorResponse } from "@angular/common/http";
import { HttpClient } from "@angular/common/http";
import { MatDialog } from "@angular/material/dialog";
import { CurrentexamComponent } from "app/layout/components/candidates/currentexam/currentexam.component";
import { SnackBarService } from 'app/layout/services/snackbar.service';

@Component({
    selector: 'cg-finish-model',
    templateUrl: './finish-model.component.html',
    styleUrls: ['./finish-model.component.scss'],
    standalone: false
})

export class FinishModelComponent implements OnInit {
  examData: any;
  public errorMsg = [];


  constructor(public dialog: MatDialog,
    @Inject(MAT_DIALOG_DATA) public parentData: any,
    private dialogRef: MatDialogRef<CurrentexamComponent>,
    private snackBar: SnackBarService) { }

  ngOnInit() {
    this.examData = this.parentData;
  }

  FinishExamModel() {    
    if (this.examData != null) {
      this.dialogRef.close(true);
    } else if (this.examData == null) {
      this.dialogRef.close();
    }
  }

  CloseDialog() {
    this.dialogRef.close();
  }

}
