import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import { HttpErrorResponse } from "@angular/common/http";
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from "@angular/material/dialog";
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { MatSelectChange } from '@angular/material/select';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { isNull, isNullOrUndefined } from 'util';
import { isEmpty } from 'lodash';
import { CandidateModel } from 'app/layout/entities/models';
import { SelectionModel } from '@angular/cdk/collections';
import { AdminService } from 'app/layout/services/admin.service';
import { SnackBarService } from 'app/layout/services/snackbar.service';
import { namePattern, emailPattern, phoneNumberPattern, genderPattern, datePattern } from 'app/layout/entities/globalConstants';
import { EditImportCandidateComponent } from './edit-import-candidate/edit-import-candidate.component';
import { FormControl, Validators, FormGroup, FormBuilder } from '@angular/forms';

@Component({
    selector: 'cg-import-candidate-list',
    templateUrl: './import-candidate-list.component.html',
    styleUrls: ['./import-candidate-list.component.scss'],
    providers: [AdminService],
    standalone: false
})

export class ImportCandidateListComponent implements OnInit {

  isSpinner: boolean;
  isSearchText: boolean;
  searchText: string;
  public searchTerm = new Subject<string>();
  public arrays: Array<CandidateModel>;
  public dataSource = new MatTableDataSource<CandidateModel>();
  public selection = new SelectionModel<CandidateModel>(true, []);

  public groups: Array<any>;
  public form: FormGroup;
  public selectedGroupId: number;

  public validationErrorCount: number = 0;

  displayedColumns: string[] = ['Select', 'FirstName', 'LastName', 'Email', 'Phone', 'DateOfBirth', 'Gender', 'action'];
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(@Inject(MAT_DIALOG_DATA) public parentData: any, private _adminService: AdminService,
    public dialogRef: MatDialogRef<any>, private snackBar: SnackBarService,
    public dialog: MatDialog, private _formBuilder: FormBuilder) { }

  ngOnInit() {

    this.form = this._formBuilder.group({
      groupId: [null, Validators.required]
    });

    if (!isNullOrUndefined(this.parentData)) {
      this.arrays = this.parentData.candidates;
      this.groups = this.parentData.groups;
      this.checkCandidates();
      this.populateCandidates();
    }

    //search box on change.
    this.searchTerm.pipe(
      debounceTime(100),
      distinctUntilChanged())
      .subscribe(value => {
        this.isSearchText = (!isEmpty(value) && !isNull(value));
        this.applyFilter(value);
      });
  }

  onGroupSelectionChange(event: MatSelectChange) {
    this.selectedGroupId = event.value;
  }

  populateCandidates() {
    this.dataSource = new MatTableDataSource(this.arrays);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  onSubmit() {
    this.showSpinner();
    this._adminService.ImportCandidates({ Candidates: this.selection.selected, GroupId: this.selectedGroupId })
      .subscribe(res => {
        this.snackBar.open('Imported ' + res.successCount + ' out of ' + res.totalCount + ' candidates', 4000);
        this.hideSpinner();
        this.dialogRef.close(true);
      }, (err: HttpErrorResponse) => {
        console.error(err.message);
        this.snackBar.error(err.message);
        this.hideSpinner();
      });

  }

  applyFilter(value: string) {
    this.dataSource.filter = value
    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  showSpinner() { this.isSpinner = true }

  hideSpinner() { this.isSpinner = false }

  clearSearch() {
    this.searchText = '';
    this.isSearchText = false;
    this.paginator.firstPage();
    this.searchTerm.next('');
  }

  editCandidate(row: CandidateModel) {
    var index = this.arrays.indexOf(row);

    const dialogRef = this.dialog.open(EditImportCandidateComponent, {
      disableClose: true,
      panelClass: ['app-no-padding-dialog'],
      data: row
    });

    dialogRef.afterClosed()
      .subscribe((result: CandidateModel) => {
        if (result) {
          this.arrays[index] = result;
          this.selection.select(result);
          this.populateCandidates();
          if (!row.isValid && result.isValid) {
            this.validationErrorCount--;
          }
        }
      })
  }

  masterToggle() {
    if (!this.selection.hasValue()) {
      this.dataSource.data.forEach(row => {
        if (row.isValid) {
          this.selection.select(row)
        }
      });
    } else {
      this.selection.clear();
    }
  }

  checkCandidates() {
    this.arrays.forEach((element, index) => {
      this.arrays[index].isValid = this.validateCandidate(element, index);
      if (this.arrays[index].isValid) {
        this.selection.select(this.arrays[index]);
      } else {
        this.validationErrorCount++;
      }
    });
  }

  validateCandidate(candidate: CandidateModel, index: number): boolean {
    
    if (isNullOrUndefined(candidate.Gender) || !candidate.Gender.match(genderPattern)) {
      return false;
    } else {
      if (candidate.Gender.toLowerCase().startsWith('m'))
        this.arrays[index].Gender = "Male";
      else if (candidate.Gender.toLowerCase().startsWith('f')) {
        this.arrays[index].Gender = "Female";
      }
    }
    if (isNullOrUndefined(candidate.FirstName) || !candidate.FirstName.match(namePattern)) {
      return false;
    }
    if (isNullOrUndefined(candidate.LastName) || !candidate.LastName.match(namePattern)) {
      return false;
    }
    if (isNullOrUndefined(candidate.Email) || !candidate.Email.match(emailPattern)) {
      return false;
    }
    if (isNullOrUndefined(candidate.Phone) || !candidate.Phone.match(phoneNumberPattern)) {
      return false;
    }
    if (!isNullOrUndefined(candidate.DateOfBirth) && candidate.DateOfBirth != '' && !candidate.DateOfBirth.match(datePattern)) {
      return false;
    }

    return true;
  }

}




