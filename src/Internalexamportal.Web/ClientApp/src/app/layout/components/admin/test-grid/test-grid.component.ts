import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from "@angular/router";
import { HttpErrorResponse } from "@angular/common/http";
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { Subject, forkJoin, of } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap, catchError } from 'rxjs/operators';
import { isEmpty } from 'lodash';
import { TestService } from 'app/layout/services/test.service';
import { SnackBarService } from 'app/layout/services/snackbar.service';
import { DeleteTestComponent } from './delete-test/delete-test.component';
import { Paginate } from 'app/layout/entities/paginate';
import { PermissionEnum } from 'app/layout/entities/permission.enum';
import { TestSetting } from 'app/layout/entities/globalConstants';

export interface TestElement {
  id: string;
  testName: string;
  duration: number;
  totalQuestion: number;
  totalMark: number;
  testUserCount: number;
}

@Component({
    selector: 'cg-test-grid',
    templateUrl: './test-grid.component.html',
    styleUrls: ['./test-grid.component.scss'],
    providers: [TestService],
    standalone: false
})
export class TestGridComponent implements OnInit {

  permissionEnum = PermissionEnum;
  public isSpinner: boolean;
  public isSearchText: boolean;
  public searchText: string;
  public searchTests = new Subject<string>();
  public paginate: Paginate = new Paginate();
  public dataSource = new MatTableDataSource<TestElement>();
  arrays: any;
  displayedColumns: string[] = ['SrNo', 'testName', 'duration', 'totalQuestion', 'totalMark', 'testUserCount', 'isResultPublished', 'Action'];

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(
    private _testService: TestService,
    private router: Router,
    public dialog: MatDialog,
    private snackBar: SnackBarService
  ) { }

  ngOnInit() {
    this.getTests();

    // Search box on change
    this.searchTests.pipe(
      debounceTime(500),
      distinctUntilChanged()
    ).subscribe(value => {
      this.isSearchText = !isEmpty(value);
      this.applyFilter(value);
    });
  }

  getTests() {
    this.showSpinner();
    this._testService.GetAllTests(this.paginate).subscribe(
      data => {
        this.arrays = data.tests;
        this.paginate.totalRecords = data.totalCount;
        this.dataSource = new MatTableDataSource(this.arrays);
      },
      (err: HttpErrorResponse) => {
        console.error(err.message);
      },
      () => this.hideSpinner()
    );
  }

  sortChange(event: any) {
    this.paginate.sortingColumn = event.active;
    this.paginate.sortingDirection = event.direction ? event.direction : (this.paginate.sortingDirection === 'asc' ? 'desc' : 'asc');
    this.paginator.firstPage();
    this.getTests();
  }

  nextOrPreviousPage(event) {
    if (this.paginate.pageSize !== event.pageSize) {
      this.paginate.pageNumber = 1;
      this.paginator.firstPage();
    } else {
      this.paginate.pageNumber = event.pageIndex + 1;
    }
    this.paginate.pageSize = event.pageSize;
    this.getTests();
  }

  openDialog(row) {
    const dialogRef = this.dialog.open(DeleteTestComponent, {
      disableClose: true,
      panelClass: ['app-no-padding-dialog'],
      data: { TestId: row.id }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) this.getTests();
    });
  }

  EditTest(row) {
    this.router.navigate(['layout/admin/edit-test', row.id]);
  }

  duplicateTest(row: any) {
    this.showSpinner();
    const originalTestId = Number(row.id);

    // Fetch test details and questions in parallel
    forkJoin({
      fullTest: this._testService.GetTestDetails(originalTestId).pipe(catchError(() => of(null))),
      originalQuestions: this._testService.GetTestQuestions(originalTestId).pipe(catchError(() => of([])))
    }).subscribe(({ fullTest, originalQuestions }) => {
      if (!fullTest) {
        this.hideSpinner();
        this.snackBar.error('Failed to fetch test details.');
        return;
      }

      // Build test settings
      const testSettings = (fullTest.testSettings || []).map(s => ({
        Id: s.id ?? s.Id,
        Type: s.type ?? s.Type ?? TestSetting[s.id],
        IsChecked: s.isChecked ?? s.IsChecked ?? false
      }));

      // Build test publish
      const tp = fullTest.testPublish || {};
      const testPublish = {
        id: 0,
        testId: 0,
        startDate: tp.startDate || new Date(),
        endDate: tp.endDate || new Date(),
        startTime: tp.startTime || '',
        endTime: tp.endTime || ''
      };

      // Build test activation
      const ta = fullTest.testActivation || {};
      const activationTypeId = ta.activationTypeId || 1;
      const commonCode = activationTypeId === 1 ? { id: 0, commonTestCode: ta.commonCode?.commonTestCode || '' } : null;
      const accessCode = activationTypeId === 2 ? { id: 0, accessTestCode: '', email: ta.accessCode?.email || '' } : null;
      const testActivation: any = { id: 0, testId: 0, activationTypeId, ...(commonCode ? { commonCode } : {}), ...(accessCode ? { accessCode } : {}) };

      // Build test instruction
      const instruction = fullTest.testDetails?.testInstruction || {};
      const testInstruction = { id: 0, name: instruction.name || '', description: instruction.description || '' };

      // Build test details
      const fd = fullTest.testDetails || {};
      const testDetails = {
        Id: 0,
        testName: (fd.testName || row.testName || 'New Test') + ' (Copy)',
        duration: fd.duration || 0,
        totalQuestion: fd.totalQuestion || (originalQuestions?.length ?? 0),
        totalMark: fd.totalMark || 0,
        testTypeId: fd.testTypeId || 1,
        testInstructionId: fd.testInstructionId || 1,
        difficultLevelId: fd.difficultLevelId || 1,
        percentage: fd.percentage || 0
      };

      // Final payload
      const newTestRequest = { testDetails, testPublish, testActivation, testSettings };
      console.log('Duplicate Test Payload:', newTestRequest);

      // Create new test
      this._testService.UpdateTestDetails(newTestRequest).subscribe(
        res => {
          if (res.success) {
            const newTestId = res.testId;
            // Map questions to new test
            const payload = {
              TestId: newTestId,
              questions: (originalQuestions || []).map(q => ({
                testId: newTestId,
                questionId: q.questionId || q.id,
                positiveMark: q.positiveMark ?? 10,
                negativeMark: q.negativeMark ?? 0
              }))
            };

                this._testService.AddTestQuestion(payload).subscribe({
              next: () => {this.getTests();},
              error: err => {
                console.error('Error adding questions:', err);
                this.snackBar.error('Failed to add questions!');
              }
            });

            this.getTests();
            this.snackBar.success("Test duplicated successfully!");
            this.router.navigate(['layout/admin/tests']);
          } else {
            this.snackBar.error(res.message);
          }
        },
        err => {
          console.error('Duplicate API error', err);
        },
        () => this.hideSpinner()
      );
    });
  }

  viewTestResults(testId: number) {
    this.router.navigate(['layout/admin/tests/test-result', testId]);
  }

  applyFilter(filterValue: string) {
    this.paginate.pageNumber = 1;
    this.paginate.searchTerm = filterValue.trim().toLowerCase();
    this.paginator.firstPage();
    this.getTests();
  }

  clearSearch() {
    this.searchText = '';
    this.paginate.searchTerm = null;
    this.isSearchText = false;
    this.paginator.firstPage();
    this.getTests();
  }

  showSpinner() { this.isSpinner = true }
  hideSpinner() { this.isSpinner = false }
}
