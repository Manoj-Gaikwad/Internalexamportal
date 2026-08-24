import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { CustomHttpService } from 'app/layout/services/custom-http.service';
import { CandidateService } from 'app/layout/services/candidate.service';
import { HttpErrorResponse, HttpEventType } from '@angular/common/http';
import { ViewTestResultComponent } from '../../admin/view-result/view-result.component';
import { DatePipe } from '@angular/common';
import { SnackBarService } from 'app/layout/services/snackbar.service';
import { TestStatus, getFileExtension, downloadFile } from 'app/layout/entities/globalConstants';
import { TestService } from 'app/layout/services/test.service';
import { AnswerSheetService } from 'app/layout/services/answer-sheet.service';
import { PrintDailogComponent } from 'app/layout/dialogs/print-dailog/print-dailog.component';

@Component({
    selector: 'cg-report',
    templateUrl: './report.component.html',
    styleUrls: ['./report.component.scss'],
    providers: [CandidateService, DatePipe, TestService],
    standalone: false
})
export class ReportComponent implements OnInit {

  public isSpinner: boolean = false;
  public testStatusEnum = TestStatus;
  Id: any;
  public dataSource;
  arrays: any;
  displayedColumns: string[] = ['id', 'testName', 'submitDate', 'attemptedQuestion', 'totalQuestion', 'skippedQuestion', 'reviewedQuestion', 'isResultPublished', 'Action'];
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;


  constructor(private router: Router,
    private _candidateService: CandidateService,
    public dialog: MatDialog,
    private snackBar: SnackBarService,
    private _testService: TestService,
    private _answerSheetService: AnswerSheetService) { }

  ngOnInit() {
    this.getCandidateTests();
  }

  getCandidateTests() {
    this.showSpinner();
    this._candidateService.GetCandidateTests()
      .subscribe(
        data => {
          this.arrays = data.tests;
          this.dataSource = new MatTableDataSource<any>(this.arrays);
          this.dataSource.sort = this.sort;
          this.dataSource.paginator = this.paginator;
          this.hideSpinner();
        },
        (err: HttpErrorResponse) => {
          console.log(err.message);
        }
      );
  }

  applyFilter(value: string) {
    this.dataSource.filter = value;
  }


  viewResult(row) {
    this.dialog.open(ViewTestResultComponent, {
      disableClose: true,
      panelClass: ['app-no-padding-dialog'],
      data: {
        Id: row.id
      }
    });
  }

  viewAnswersheet(row) {
    this._answerSheetService.submittedTestId = row.id;
    this.router.navigate(['/layout/candidates/answer-sheet']);
  }

  printCertificate(row) {
    this.showSpinner();
    this._testService.PrintCertificate(row.id)
      .subscribe(
        (data: any) => {
          if (data) {
            if (data.type == HttpEventType.Response) {
              var extension = getFileExtension(data.body.type);
              var fileName = `CandidateCertificate${extension}`;
              this.openPopUp(data, fileName);
              this.hideSpinner();
              return;
            }
            //this.hideSpinner();
          }
          //this.hideSpinner();
        },
        err => {
          this.hideSpinner();
          let errorMsg = err.error[""][0];
        }
      );
  }
  
  openPopUp(data:any, model){
    if(!data || data.type != HttpEventType.Response) return;

    this.dialog.open(PrintDailogComponent, {
      data: {
        fileData:data,
        fileName:model
      },
      panelClass:['app-no-padding-dialog'],
      disableClose:true
    });
  }
  
  showSpinner() { this.isSpinner = true }

  hideSpinner() { this.isSpinner = false }

}
