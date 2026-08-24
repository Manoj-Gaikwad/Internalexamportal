import { Component, OnInit } from '@angular/core';
import { CustomHttpService } from "app/layout/services/custom-http.service";
import { Router } from "@angular/router";
import { MAT_DIALOG_DATA, MatDialog } from "@angular/material/dialog";
import { Inject } from "@angular/core";
import { SnackBarService } from 'app/layout/services/snackbar.service';
import { TestService } from 'app/layout/services/test.service';
import { HttpEventType } from '@angular/common/http';
import { getFileExtension, downloadFile } from 'app/layout/entities/globalConstants';
import { DecimalPipe } from '@angular/common';
import { PrintDailogComponent } from 'app/layout/dialogs/print-dailog/print-dailog.component';

@Component({
    selector: 'cg-view-result',
    templateUrl: './view-result.component.html',
    styleUrls: ['./view-result.component.scss'],
    providers: [TestService, DecimalPipe],
    standalone: false
})
export class ViewTestResultComponent implements OnInit {

  public isSpinner: boolean = false;
  Id: any;
  errorMsg: any;
  result: any;

  constructor(private router: Router,
    private _testService: TestService,
    @Inject(MAT_DIALOG_DATA) public parentData: any,
    private snackBar: SnackBarService,
    private dialog: MatDialog) { }

  ngOnInit() {
    this.showSpinner();
    this.Id = this.parentData.Id;
    this._testService.GetTestResultById(this.Id)
      .subscribe(
        (res: any) => {
          this.result = res;
          this.hideSpinner();
        },
        err => {
          this.errorMsg = err.error[""][0];
        }
      );
  }

  printResult() {
  this.showSpinner();
  this._testService.PrintCandidateResult(this.Id)
    .subscribe(
      (data: Blob) => {
        if (data) {
          // Extract file extension from MIME type
          const extension = getFileExtension(data.type);
          const fileName = `CandidateResult${extension}`;

          // Trigger download
          const url = window.URL.createObjectURL(data);
          const a = document.createElement('a');
          a.href = url;
          a.download = fileName;
          document.body.appendChild(a);
          a.click();
          document.body.removeChild(a);
          window.URL.revokeObjectURL(url);
          this.openPopUp(data, fileName);
          this.hideSpinner();
        }
      },
      (err) => {
        this.hideSpinner();
        console.error('Error downloading result:', err);
      }
    );
}


  showSpinner() { this.isSpinner = true }

  hideSpinner() { this.isSpinner = false }


  openPopUp(data: any, model) {
    if (!data || data.type != HttpEventType.Response) return;

    this.dialog.open(PrintDailogComponent, {
      data: {
        fileData: data,
        fileName: model
      },
      panelClass: ['app-no-padding-dialog'],
      disableClose: true
    });
  }
}
