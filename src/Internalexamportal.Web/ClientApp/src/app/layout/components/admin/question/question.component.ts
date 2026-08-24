import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";
import { HttpErrorResponse } from "@angular/common/http";
import { fromEvent, Subject, ReplaySubject } from "rxjs";
import { takeUntil, debounceTime, distinctUntilChanged } from "rxjs/operators";
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { MatSelectChange } from '@angular/material/select';
import { CustomHttpService } from "app/layout/services/custom-http.service";
import { FormGroup, Validators, FormControl } from "@angular/forms";
import { FormBuilder } from "@angular/forms";
import { QuestionService } from 'app/layout/services/question.service';
import { QuestionPaginate } from 'app/layout/entities/paginate';
import { isNull } from 'util';
import { isEmpty } from 'lodash';
import { PermissionEnum } from 'app/layout/entities/permission.enum';
import { QuestionAddComponent } from './question-add/question-add.component';
import { DeleteQuestionComponent } from './delete-question/delete-question.component';


export interface QuestionElement {
  id: number;
  description: string;
  subject: string;
  topic: string;
  type: string;
  questionMark: any;
}

var arr: QuestionElement[];
@Component({
    selector: 'cg-question',
    templateUrl: './question.component.html',
    styleUrls: ['./question.component.scss'],
    providers: [QuestionService],
    standalone: false
})
export class QuestionComponent {

  permissionEnum = PermissionEnum;
  public isSpinner: boolean;
  public subjects: Array<any> = [];
  public subjectTopics: Array<any> = [];

  public subjectSearchControl: FormControl = new FormControl();
  public toggleAllSubjectsStatus: boolean = false;
  public filteredSubjects: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);

  public topicSearchControl: FormControl = new FormControl();
  public toggleAllTopicsStatus: boolean = false;
  public filteredTopics: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);

  protected _onDestroy = new Subject<void>();

  public paginate: QuestionPaginate = new QuestionPaginate();
  public searchQuestions = new Subject<string>();
  public isSearchText: boolean;
  public errorMsg: any;
  public QuestionType: any;
  _unsubscribeAll: any;
  public dataSource = new MatTableDataSource<QuestionElement>();
  arrays: any;
  SubjectForm: FormGroup;
  displayedColumns: string[] = ['id', 'description', 'subject', 'topic', 'type', 'action'];
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(private router: Router, public dialog: MatDialog,
    private _formBuilder: FormBuilder, private _questionService: QuestionService) {

    this.SubjectForm = this._formBuilder.group({
      SubjectId: [null],
      SubjectTopicId: [null],
      QusetionTypeId: [null],
      searchTerm: [null]

    });

  }
  ngOnInit(): void {

    this.getAllQuestions();

    this.getAllQuestionTypes();

    this.getAllSubjectsWithTopics();

    //search box on change.
    this.searchQuestions.pipe(
      debounceTime(500),
      distinctUntilChanged())
      .subscribe(value => {
        this.isSearchText = (!isEmpty(value) && !isNull(value));
        this.applyFilter(value);
      });

    this.onMatSelectSearchChange();
  }

  onMatSelectSearchChange() {
    this.subjectSearchControl.valueChanges
      .pipe(takeUntil(this._onDestroy))
      .subscribe((val) => {
        this.filterSubjectChange(val);
      });
    this.topicSearchControl.valueChanges
      .pipe(takeUntil(this._onDestroy))
      .subscribe((val) => {
        this.filterTopicChange(val);
      });
  }

  protected filterSubjectChange(search) {
    if (isNull(search) || (typeof search !== 'string')) {
      this.filteredSubjects.next(this.subjects.slice());
      return;
    } else search = search.toLowerCase();

    this.filteredSubjects.next(
      this.subjects.filter(type => type.subjectName.toLowerCase().indexOf(search) > -1)
    );
  }

  protected filterTopicChange(search) {

    const subjectTopicsCopy = this.copySubjectTopics(this.subjectTopics);

    if (isNull(search) || (typeof search !== 'string')) {
      this.filteredTopics.next(subjectTopicsCopy.slice());
      return;
    } else search = search.toLowerCase();

    this.filteredTopics.next(
      subjectTopicsCopy.filter(subject => {
        const showSubjectGroup = subject.subjectName.toLowerCase().indexOf(search) > -1;
        if (!showSubjectGroup) {
          subject.subjectTopic = subject.subjectTopic.filter(topic => topic.topic.toLowerCase().indexOf(search) > -1);
        }
        return subject.subjectTopic.length > 0;
      })
    );
  }

  private copySubjectTopics(subjectTopics) {
    const subjectTopicsCopy = [];
    subjectTopics.forEach(subject => {
      subjectTopicsCopy.push({
        id: subject.id,
        subjectName: subject.subjectName,
        subjectTopic: subject.subjectTopic.slice()
      });
    });
    return subjectTopicsCopy;
  }

  toggleAllSubjects(event) {
    if (event) {
      const subjectIds = this.subjects.map(prop => prop.id);
      this.SubjectForm.get('SubjectId').setValue(subjectIds);
      this.toggleAllSubjectsStatus = true;

      this.subjectTopics = this.subjects.filter(prop => subjectIds.includes(prop.id));
      this.SubjectForm.get('SubjectTopicId').patchValue([]);
      this.filteredTopics.next(this.subjectTopics.slice());
      this.toggleAllTopicsStatus = false;
    } else {
      this.SubjectForm.get('SubjectId').setValue(null);
      this.SubjectForm.get('SubjectTopicId').setValue(null);
      this.filteredTopics.next([]);
      this.toggleAllSubjectsStatus = false;
      this.toggleAllTopicsStatus = false;
    }
  }

  toggleAllTopics(event) {
    if (event) {
      var topicIds: Array<any> = [];
      this.subjectTopics.forEach(sub => {
        const ids =sub.subjectTopic.map(prop => prop.id);
        topicIds = topicIds.concat(ids);
      });
      this.SubjectForm.get('SubjectTopicId').setValue(topicIds);
      this.toggleAllTopicsStatus = true;
    } else {
      this.SubjectForm.get('SubjectTopicId').setValue(null);
      this.toggleAllTopicsStatus = false;
    }
  }

  getAllQuestions() {
    this.showSpinner();

    var Form = this.SubjectForm.value;
    this.paginate.subjectTopicIds = Form.SubjectTopicId;
    this.paginate.questionTypeIds = Form.QusetionTypeId;
    this.paginate.subjectIds = Form.SubjectId;

    this._questionService.GetQuestionsByFilter(this.paginate).subscribe(
      data => {
        this.arrays = data.questions;
        this.paginate.totalRecords = data.totalCount;
        this.dataSource = new MatTableDataSource(this.arrays);
      },
      (err: HttpErrorResponse) => {
        console.error(err.message);
      }, () => {
        this.hideSpinner();
      }
    );
  }

  getAllSubjectsWithTopics() {
    this._questionService.GetAllSubjectsWithTopics()
      .subscribe(res => {
        this.subjects = res;
        this.filteredSubjects.next(this.subjects.slice());
      }, err => {
        console.error(err);
      })
  }

  getAllQuestionTypes() {
    this._questionService.GetQuestionTypes().subscribe(
      data => {
        this.QuestionType = data;
      },
      (err: HttpErrorResponse) => {
        console.log(err.message);
      }
    );
  }

  openAddQuestionDialog() {
    const dialogRef = this.dialog.open(QuestionAddComponent, {
      disableClose: true,
      restoreFocus: false,
      width: '80%',
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.getAllQuestions();
      }
    });
  }

  openEditQuestionDialog(row) {
    const dialogRef = this.dialog.open(QuestionAddComponent, {
      disableClose: true,
      width: '80%',
      restoreFocus: false,
      data: {
        id: row.id
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.getAllQuestions();
      }
    });
  }

  onSubjectSelectionChange(subject: MatSelectChange) {
    this.subjectTopics = this.subjects.filter(prop => subject.value.includes(prop.id));
    this.SubjectForm.get('SubjectTopicId').patchValue([]);
    this.filteredTopics.next(this.subjectTopics.slice());
    this.toggleAllTopicsStatus = false;
  }

  sortChange(event: any) {
    var column = event.active
    this.paginate.sortingColumn = column;
    this.paginate.sortingDirection =
      event.direction ? event.direction : this.paginate.sortingDirection == 'asc' ? 'desc' : 'asc';
    this.paginator.firstPage();
    this.getAllQuestions();
  }

  nextOrPreviousPage(event) {
    if (this.paginate.pageSize != event.pageSize) {
      this.paginate.pageNumber = 1;
      this.paginator.firstPage();
    } else {
      this.paginate.pageNumber = event.pageIndex + 1;
    }
    this.paginate.pageSize = event.pageSize;
    this.getAllQuestions();
  }

  applyFilter(value: string) {
    this.paginate.pageNumber = 1;
    this.paginate.searchTerm = value.trim().toLowerCase();
    this.paginator.firstPage();
    this.getAllQuestions();

  }

  resetFilter() {
    this.SubjectForm.get('searchTerm').patchValue('');
    this.isSearchText = false;
    this.paginate = new QuestionPaginate();
    this.paginator.firstPage();
    this.getAllQuestions();
  }

  openDialog(row) {
    const dialogRef = this.dialog.open(DeleteQuestionComponent, {
      disableClose: true,
      panelClass: ['app-no-padding-dialog'],
      data: {
        QuestionId: row.id
      }
    });

    dialogRef.afterClosed()
      .subscribe(res => {
        if (res) {
          this.getAllQuestions();
        }
      })

  }

  onSubmit() {
    this.getAllQuestions();
  }

  resetSearchFilter() {
    this.filteredTopics.next([]);
    this.subjectTopics = [];
    this.isSearchText = false;
    this.paginate = new QuestionPaginate();
    this.paginator.firstPage();
    this.SubjectForm.reset();
  }

  showSpinner() { this.isSpinner = true }

  hideSpinner() { this.isSpinner = false }


  getRowIndex(i: number): number {
    return (this.paginator.pageIndex == 0 ? i + 1 : 1 + i + this.paginator.pageIndex * this.paginator.pageSize);
  }

  getAbbriviation(type: string): string {
    if (type == 'Multiple Choice Question') {
      return 'MCQ';
    } else {
      return type;
    }
  }

  ngOnDestroy() {
    this._onDestroy.next();
    this._onDestroy.complete();
  }
}


