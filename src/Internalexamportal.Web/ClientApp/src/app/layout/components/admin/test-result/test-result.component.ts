import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { HttpErrorResponse, HttpEventType } from "@angular/common/http";
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { ViewChild } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { Location } from '@angular/common';
import { ViewTestResultComponent } from '../view-result/view-result.component';
import { TestService } from 'app/layout/services/test.service';
import { SnackBarService } from 'app/layout/services/snackbar.service';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { isEmpty, parseInt } from 'lodash';
import { isNull } from 'util';
import { getFileExtension, downloadFile, TestStatus } from 'app/layout/entities/globalConstants';
import { Paginate, SubmittedTestPaginate } from 'app/layout/entities/paginate';
import { PrintDailogComponent } from 'app/layout/dialogs/print-dailog/print-dailog.component';
import { SubmittedTestService } from 'app/layout/services/submitted-test.service';
import { Console } from 'console';


export interface TestResult {
  id: number;
  maximumMark: number;
  totalMark: number;
  userName: String;
  status: string;
}

@Component({
    selector: 'cg-test-result',
    templateUrl: './test-result.component.html',
    styleUrls: ['./test-result.component.scss'],
    providers: [TestService, SubmittedTestService],
    standalone: false
})

export class TestResultComponent implements OnInit {

  public isSpinner: boolean = false;

  public isSearchText: boolean;
  public searchText: string;
  public searchTerm = new Subject<string>();
  public testStatusEnum = TestStatus;
  public dataSource: MatTableDataSource<TestResult>;
  public arrays: Array<TestResult>;
  public testName: string = "Test";
  public paginate = new SubmittedTestPaginate();

  displayedColumns: string[] = ['id', 'name', 'submitDate', 'obtainedMarks', 'maximumMark', 'status', 'Action'];
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;


  constructor(private router: Router,
    private _testService: TestService,
    public dialog: MatDialog,
    private snackBar: SnackBarService,
    private route: ActivatedRoute,
    private _location: Location,
    private _submittedTestService: SubmittedTestService) { }


  ngOnInit() {
    let id = this.route.snapshot.paramMap.get('testId');
    if (id) {
      this.paginate.testId = parseInt(id);
    } else {
      this.router.navigate(['/layout/admin/tests']);
    }
    this.getTestResults()

    //search box on change.
    this.searchTerm.pipe(
      debounceTime(500),
      distinctUntilChanged())
      .subscribe(value => {
        this.isSearchText = (!isEmpty(value) && !isNull(value));
        this.applyFilter(value);
      });
  }

  getTestResults() {
    this.showSpinner();
    this._testService.GetTestResults(this.paginate).subscribe(
      data => {
        this.arrays = data.testResult;
        this.testName = data.testName;
        this.paginate.totalRecords = data.totalCount;
        this.dataSource = new MatTableDataSource<TestResult>(this.arrays);
        this.hideSpinner();
      },
      (err: HttpErrorResponse) => {
        console.log(err.message);
        this.hideSpinner();
      }
    );
  }

  sortChange(event: any) {
    var column = event.active
    this.paginate.sortingColumn = column;
    this.paginate.sortingDirection =
      event.direction ? event.direction : this.paginate.sortingDirection == 'asc' ? 'desc' : 'asc';
    this.paginator.firstPage();
    this.getTestResults();
  }

  nextOrPreviousPage(event) {
    if (this.paginate.pageSize != event.pageSize) {
      this.paginate.pageNumber = 1;
      this.paginator.firstPage();
    } else {
      this.paginate.pageNumber = event.pageIndex + 1;
    }
    this.paginate.pageSize = event.pageSize;
    this.getTestResults();
  }

  sendResultEmail() {
    this.showSpinner();
    var url = window.location.origin + window.location.pathname
    this._testService.SendExamResultEmail({ testId: this.paginate.testId, websiteUrl: url })
      .subscribe(
        data => {
          this.hideSpinner();
          if (data.success) {
            this.snackBar.success('Test result sent to email successfully');
          } else {
            this.snackBar.error('Error sending email.');
          }
        },
        (err: HttpErrorResponse) => {
          console.log(err.message);
          this.hideSpinner();
        }
      );
  }

  allowTestResume(row) {
    this.showSpinner();
    this._submittedTestService.allowTestResume(row.id)
      .subscribe(
        data => {
          this.hideSpinner();
          if (data.success) {
            if (!row.allowTestResume)
              this.snackBar.success('Enabled test resuming successfully.');
            else
              this.snackBar.success('Disabled test resuming successfully.');
            this.getTestResults();
          } else {
            this.snackBar.error('Error enabling test resume.');
          }
        },
        (err: HttpErrorResponse) => {
          console.log(err.message);
          this.hideSpinner();
        }
      );
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

  clearSearch() {
    this.searchText = '';
    this.paginate.searchTerm = null;
    this.isSearchText = false;
    this.paginator.firstPage();
    this.getTestResults();
  }

  applyFilter(value: string) {
    this.paginate.pageNumber = 1;
    this.paginate.searchTerm = value.trim().toLowerCase();
    this.paginator.firstPage();
    this.getTestResults();
  }


  sendEmailToCandidate(row, sendEmailDialog) {
    const dialogRef = this.dialog.open(sendEmailDialog, {
      width: '400px',
      panelClass: ['app-no-padding-dialog'],
      disableClose: true,
      restoreFocus: false
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.showSpinner();
        var url = window.location.origin + window.location.pathname
        this._testService.SendExamResultEmailToCandidate({ id: row.id, websiteUrl: url })
          .subscribe(
            data => {
              this.hideSpinner();
              if (data.success) {
                this.snackBar.success('Test result sent to email successfully');
              } else {
                this.snackBar.error('Error sending email.');
              }
            },
            (err: HttpErrorResponse) => {
              console.log(err.message);
              this.hideSpinner();
            }
          );
      }
    })
  }

  openSendEmailDialog(sendEmailDialog) {
    const dialogRef = this.dialog.open(sendEmailDialog, {
      width: '400px',
      panelClass: ['app-no-padding-dialog'],
      disableClose: true,
      restoreFocus: false
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.sendResultEmail();
      }
    })
  }

  goBack() {
    this._location.back();
  }

  getRowIndex(i: number): number {
    return (this.paginator.pageIndex == 0 ? i + 1 : 1 + i + this.paginator.pageIndex * this.paginator.pageSize);
  }

  showSpinner() { this.isSpinner = true }

  hideSpinner() { this.isSpinner = false }

  // printCertificate(row) {
  //   this.showSpinner();
  //   this._testService.PrintCertificate(row.id)
  //     .subscribe(
  //       (data: any) => {
  //         if (data) {
  //           if (data.type == HttpEventType.Response) {
  //             var extension = getFileExtension(data.body.type);
  //             var fileName = `CandidateCertificate${extension}`;
  //             this.openPopUp(data, fileName);
  //             this.hideSpinner();
  //             return;
  //           }
  //           this.hideSpinner();
  //         }
  //         //this.hideSpinner();
  //       },
  //       err => {
  //         this.hideSpinner();
  //         let errorMsg = err.error[""][0];
  //       }
  //     );
  // }

  printCertificate(row: any) {
  this.showSpinner();
  this._testService.PrintCertificate(row.id)
    .subscribe(
      (data: Blob) => {
        if (data) {
          // Determine file extension from MIME type
          const extension = getFileExtension(data.type);
          const fileName = `CandidateCertificate${extension}`;

          // Trigger download
          const url = window.URL.createObjectURL(data);
          const a = document.createElement('a');
          a.href = url;
          a.download = fileName;
          document.body.appendChild(a);
          a.click();
          document.body.removeChild(a);
          window.URL.revokeObjectURL(url);

          this.hideSpinner();
        }
      },
      (err) => {
        this.hideSpinner();
        console.error('Error downloading file:', err);
      }
    );
}
printReport() {
    this.showSpinner();
    this._testService.PrintTestReport(this.paginate.testId)
        .subscribe({
            next: (res: Blob) => {
                const extension = getFileExtension(res.type);
                const fileName = `TestReport${extension}`;
                const url = window.URL.createObjectURL(res);
                const a = document.createElement('a');
                a.href = url;
                a.download = fileName;
                a.click();
                window.URL.revokeObjectURL(url);
                this.hideSpinner();
            },
            error: (err) => {
                console.error(err);
                this.hideSpinner();
            }
        });
}

PrintReportExcel() {
  this.showSpinner();
  this._testService.PrintTestReportExcel(this.paginate.testId)
      .subscribe({
          next: (res: Blob) => {
              const fileName = `TestReport.xlsx`;
              const url = window.URL.createObjectURL(res);
              const a = document.createElement('a');
              a.href = url;
              a.download = fileName;
              a.click();
              window.URL.revokeObjectURL(url);
              this.hideSpinner();
          },
          error: (err) => {
              console.error(err);
              this.hideSpinner();
          }
      });
}



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
