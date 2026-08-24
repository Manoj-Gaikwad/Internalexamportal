import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { HttpErrorResponse } from "@angular/common/http";
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { MatSort } from '@angular/material/sort';
import { ViewChild } from "@angular/core";

import { QuestionService } from 'app/layout/services/question.service';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { isEmpty } from 'lodash';
import { isNull } from 'util';
import { PermissionEnum } from 'app/layout/entities/permission.enum';
import { SubjectAddComponent } from './subject-add/subject-add.component';
import { DeletesubjectComponent } from './deletesubject/deletesubject.component';


export interface SubjectElement {
  SrNo: string;
  Name: string;
  Action: string
}
var arr: SubjectElement[];
@Component({
    selector: 'cg-subject-details',
    templateUrl: './subject-details.component.html',
    styleUrls: ['./subject-details.component.scss'],
    providers: [QuestionService],
    standalone: false
})
export class SubjectDetailsComponent implements OnInit {
  
  permissionEnum = PermissionEnum;
  isSpinner: boolean;
  isSearchText: boolean;
  searchText: string;
  public searchTerm = new Subject<string>();
  subject: any;
  public dataSource = new MatTableDataSource<SubjectElement>(arr);
  arrays: any;

  displayedColumns: string[] = ['SrNo', 'subjectName', 'Action'];
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(private _questionService: QuestionService, private router: Router, public dialog: MatDialog) { }

  ngOnInit() {
    this.getAllSubjects();

    //search box on change.
    this.searchTerm.pipe(
      debounceTime(100),
      distinctUntilChanged())
      .subscribe(value => {
        this.isSearchText = (!isEmpty(value) && !isNull(value));
        this.applyFilter(value);
      });
  }

  getAllSubjects() {
    this.showSpinner()
    this._questionService.GetAllSubjects().subscribe(
      data => {
        this.arrays = data;
        this.dataSource = new MatTableDataSource(this.arrays);
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
      },
      (err: HttpErrorResponse) => {
        console.log(err.message);
      }, () => {
        this.hideSpinner();
      }
    );
  }

  AddSubject() {
    const dialogRef = this.dialog.open(SubjectAddComponent, {
      width: '400px',
      panelClass: ['app-no-padding-dialog'],
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.getAllSubjects();
      }
    });
  }

  EditSubject(row) {
    const dialogRef = this.dialog.open(SubjectAddComponent, {
      width: '400px',
      panelClass: ['app-no-padding-dialog'],
      data: {
        id: row.id
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.getAllSubjects();
      }
    });
  }

  viewTopics(row) {
    this.subject = row;
    this.router.navigate(['layout/admin/subject-edit'], {
      queryParams: {
        id: this.subject.id,
        subjectName: this.subject.subjectName
      }
    });
  }

  applyFilter(value: string) {
    this.dataSource.filter = value
    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  openDialog(row) {
    const dialogRef = this.dialog.open(DeletesubjectComponent, {
      disableClose: true,
      panelClass: ['app-no-padding-dialog'],
      data: {
        SubjectId: row.id
      }
    });

    dialogRef.afterClosed().subscribe(res => {
      if (res)
        this.getAllSubjects();
    });
  }


	getRowIndex(i: number): number {
		return (this.paginator.pageIndex == 0 ? i + 1 : 1 + i + this.paginator.pageIndex * this.paginator.pageSize);
	}

  showSpinner() { this.isSpinner = true }

  hideSpinner() { this.isSpinner = false }


  clearSearch() {
    this.searchText = '';
    this.isSearchText = false;
    this.paginator.firstPage();
    this.getAllSubjects();
  }

}


