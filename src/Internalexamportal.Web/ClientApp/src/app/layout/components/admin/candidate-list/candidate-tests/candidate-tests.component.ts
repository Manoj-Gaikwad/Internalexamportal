import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { CandidateService } from 'app/layout/services/candidate.service';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { SnackBarService } from 'app/layout/services/snackbar.service';
import { HttpErrorResponse } from '@angular/common/http';
import { ViewTestResultComponent } from '../../view-result/view-result.component';

@Component({
    selector: 'candidate-tests',
    templateUrl: './candidate-tests.component.html',
    styleUrls: ['./candidate-tests.component.scss'],
    providers: [CandidateService],
    standalone: false
})
export class CandidateTestsComponent implements OnInit {

  @Input() candidateId: string;
  public isSpinner: boolean = false;
  public dataSource = new MatTableDataSource<any>();
  arrays: any;
  displayedColumns: string[] = ['id', 'testName', 'submitDate', 'totalMark', 'maximumMark', 'rightMark', 'negativeMark', 'Action'];
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;


  constructor(private router: Router,
    private _candidateService: CandidateService,
    public dialog: MatDialog,
    private snackBar: SnackBarService) { }

  ngOnInit() {
    this.getCandidateTests();
  }

  getCandidateTests() {
    this.isSpinner = true;
    this._candidateService.GetCandidateTestsById(this.candidateId)
      .subscribe(
        data => {
          this.isSpinner = false;
          this.arrays = data.tests;
          this.dataSource = new MatTableDataSource<any>(this.arrays);
          this.dataSource.sort = this.sort;
          this.dataSource.paginator = this.paginator;
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

}
