import { Component, OnInit, ViewChild } from '@angular/core';
import { CandidateService } from 'app/layout/services/candidate.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { ViewTestResultComponent } from '../../admin/view-result/view-result.component';
import { SnackBarService } from 'app/layout/services/snackbar.service';

@Component({
    selector: 'cg-upcoming-tests',
    templateUrl: './upcoming-tests.component.html',
    styleUrls: ['./upcoming-tests.component.scss'],
    providers: [CandidateService],
    standalone: false
})
export class UpcomingTestsComponent implements OnInit {

  public isSpinner: boolean = false;

  Id: any;
  public dataSource;
  arrays: any;
  displayedColumns: string[] = ['id', 'testName','duration','totalMark','totalQuestion','startDate','endDate', 'Action'];
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
    this.showSpinner();
    this._candidateService.GetUpcomingTests()
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

  
  openActivationWindow(){
    
    var url = window.location.origin + window.location.pathname + '#/layout/candidates/activation' ;
    
    var myWindow = window.open(url, "", "width=2050,height=1000");
  }

  showSpinner() { this.isSpinner = true }

  hideSpinner() { this.isSpinner = false }
}
