import { Component, OnInit, ViewChild } from '@angular/core';
import { HttpErrorResponse } from "@angular/common/http";
import { HttpClient } from "@angular/common/http";
import { Router } from "@angular/router";
import { CustomHttpService } from "app/layout/services/custom-http.service";
import { FormBuilder } from "@angular/forms";
import { DataServicesService } from "app/layout/services/data-services.service";
import { MatDialog } from "@angular/material/dialog";
import { BlankComponent } from "app/layout/components/blank/blank.component";
import { ActivatedRoute } from "@angular/router";
import { QuestionService } from 'app/layout/services/question.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { Location } from '@angular/common';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { isNull } from 'util';
import { isEmpty } from 'lodash';
import { PermissionEnum } from 'app/layout/entities/permission.enum';
import { SnackBarService } from 'app/layout/services/snackbar.service';
import { TopicAddEditComponent } from '../topic-add-edit/topic-add-edit.component';

@Component({
    selector: 'cg-edit-subject',
    templateUrl: './edit-subject.component.html',
    styleUrls: ['./edit-subject.component.scss'],
    providers: [QuestionService],
    standalone: false
})
export class EditSubjectComponent implements OnInit {
  permissionEnum = PermissionEnum;
  isSpinner: boolean;
  isSearchText: boolean;
  searchText: string;
  public searchTerm = new Subject<string>();
  subjectName: any;
  subjectId: any;
  subject: any;
  TopicForm: any;
  errorMsg: any;
  Id: number;
  public dataSource = new MatTableDataSource<any>();
  arrays: any;

  displayedColumns: string[] = ['SrNo', 'topic', 'questions','Action'];
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(private _questionService: QuestionService, private _formBuilder: FormBuilder,
    private _location: Location, private router: Router, public dialog: MatDialog,
    private activatedRoute: ActivatedRoute, private snackBar: SnackBarService) { }

  ngOnInit() {
    this.subjectId = this.activatedRoute.snapshot.queryParams['id'];
    this.subjectName = this.activatedRoute.snapshot.queryParams['subjectName'];
    this.getSubjectTopics();

    //search box on change.
    this.searchTerm.pipe(
      debounceTime(100),
      distinctUntilChanged())
      .subscribe(value => {
        this.isSearchText = (!isEmpty(value) && !isNull(value));
        this.applyFilter(value);
      });
  }

getSubjectTopics() {
  this.showSpinner();

  this._questionService.GetTopic(this.subjectId)
    .subscribe(
      res => {
        console.log(res);

        // Remove <p> tags from question descriptions
        this.arrays = res.map(topic => {
          if (topic.questionsData) {
            topic.questionsData = topic.questionsData.map(q => ({
              ...q,
              description: q.description.replace(/<\/?p>/g, '').trim()
            }));
          }
          return topic;
        });

        this.dataSource = new MatTableDataSource(this.arrays);
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
        this.isSpinner = false;
      },
      err => {
        console.error(err);
      },
      () => {
        this.hideSpinner();
      }
    );
}


  openAddTopicDialog() {
    const dialogRef = this.dialog.open(TopicAddEditComponent, {
      width: '400px',
      panelClass: ['app-no-padding-dialog'],
      data: {
        subjectId: this.subjectId
      }
    })

    dialogRef.afterClosed()
      .subscribe(result => {
        if (result) {
          this.getSubjectTopics();
        }
      })
  }

  editTopic(row) {
    const dialogRef = this.dialog.open(TopicAddEditComponent, {
      width: '400px',
      panelClass: ['app-no-padding-dialog'],
      data: {
        subjectId: this.subjectId,
        id: row.id,
        topic: row.topic
      }
    });

    dialogRef.afterClosed()
      .subscribe(result => {
        if (result) {
          this.getSubjectTopics();
        }
      })
  }


  applyFilter(value: string) {
    this.dataSource.filter = value
    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }


  showSpinner() { this.isSpinner = true }

  hideSpinner() { this.isSpinner = false }

	getRowIndex(i: number): number {
		return (this.paginator.pageIndex == 0 ? i + 1 : 1 + i + this.paginator.pageIndex * this.paginator.pageSize);
	}

  clearSearch() {
    this.searchText = '';
    this.isSearchText = false;
    this.paginator.firstPage();
    this.getSubjectTopics();
  }

  deleteTopic(row) {

  }

  goBack() {
    this._location.back();
  }
}




