
import { Component, OnInit, ViewChild } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Router } from "@angular/router";
import { HttpErrorResponse } from "@angular/common/http";
import { MatTableDataSource } from "@angular/material/table";
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from "@angular/material/paginator";
import { CustomHttpService } from "app/layout/services/custom-http.service";
import { DeleteCandidateComponent } from "app/layout/components/candidates/delete-candidate/delete-candidate.component";
import { MatDialog } from "@angular/material/dialog";
import { AdminService } from 'app/layout/services/admin.service';
import { Paginate } from 'app/layout/entities/paginate';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { isEmpty } from 'lodash';
import { isNull } from 'util';
import { PermissionEnum } from 'app/layout/entities/permission.enum';
import { SnackBarService } from 'app/layout/services/snackbar.service';
import { AddAdminComponent } from './add-admin/add-admin.component';

@Component({
    selector: 'cg-admin-manage',
    templateUrl: './admin-manage.component.html',
    styleUrls: ['./admin-manage.component.scss'],
    providers: [AdminService],
    standalone: false
})
export class AdminManageComponent implements OnInit {

  permissionEnum = PermissionEnum;
  Id: any;
  _unsubscribeAll: any;
  public dataSource = new MatTableDataSource<any>();
  arrays: any;
  candidateData: any;
  public getName: string;
  public errorMsg = [];

  public isSearchText: boolean;
  public searchText: string;
  public searchTerm = new Subject<string>();
  public paginate = new Paginate();
  public isSpinner: boolean = false;

  displayedColumns: string[] = ['id','name', 'email', 'phoneNumber','role','status', 'action'];
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(private router: Router, public dialog: MatDialog,
    private adminService: AdminService,private snackBar: SnackBarService) { }

  ngOnInit() {
    this.getAllAdmins();

    //search box on change.
    this.searchTerm.pipe(
      debounceTime(500),
      distinctUntilChanged())
      .subscribe(value => {
        this.isSearchText = (!isEmpty(value) && !isNull(value));
        this.applyFilter(value);
      });
  }

  getAllAdmins() {
    this.showSpinner()
    this.adminService.GetAllAdmins(this.paginate).subscribe(
      (data: any) => {
        this.arrays = data.admins;
        this.paginate.totalRecords = data.totalCount;
        this.dataSource = new MatTableDataSource(this.arrays);
      },
      (err: HttpErrorResponse) => {
        console.log(err.message);
      }, () => {
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
    this.getAllAdmins();
  }


  nextOrPreviousPage(event) {
    if (this.paginate.pageSize != event.pageSize) {
      this.paginate.pageNumber = 1;
      this.paginator.firstPage();
    } else {
      this.paginate.pageNumber = event.pageIndex + 1;
    }
    this.paginate.pageSize = event.pageSize;
    this.getAllAdmins();
  }

  showSpinner() { this.isSpinner = true }

  hideSpinner() { this.isSpinner = false }


  clearSearch() {
    this.searchText = '';
    this.paginate.searchTerm = null;
    this.isSearchText = false;
    this.paginator.firstPage();
    this.getAllAdmins();
  }

  applyFilter(value: string) {
    this.paginate.pageNumber = 1;
    this.paginate.searchTerm = value.trim().toLowerCase();
    this.paginator.firstPage();
    this.getAllAdmins();
  }

  DeleteAdmin(row) {

    const dialogRef = this.dialog.open(DeleteCandidateComponent, {
      disableClose: true,
			panelClass: ['app-no-padding-dialog'],
      data: {
        CandidateId: row.id
      }
    });
    dialogRef.afterClosed()
      .subscribe(result => {
        if (result) {
          this.getAllAdmins();
        }
      })
  }

	getRowIndex(i:number):number{
		return (this.paginator.pageIndex == 0 ? i + 1 : 1 + i + this.paginator.pageIndex * this.paginator.pageSize);
	}
  
	toggleActiveStatus(user) {
		this.adminService.changeUserStatus(user.id)
			.subscribe((res: any) => {
				this.snackBar.success(`User made ${(user.isActive ? 'Inactive' : 'Active')} Successfully.`);
				this.getAllAdmins();
			}, () => { }
			)

	}

  openAddAdminDialog() {

    const dialogRef = this.dialog.open(AddAdminComponent, {
      disableClose: true,
      panelClass: ['app-no-padding-dialog']
    });

    dialogRef.afterClosed()
      .subscribe(result => {
        if (result) {
          this.getAllAdmins();
        }
      })
  }

  EditAdmin(row) {

    const dialogRef = this.dialog.open(AddAdminComponent, {
      disableClose: true,
      panelClass: ['app-no-padding-dialog'],
      data: {
        id: row.id
      }
    });

    dialogRef.afterClosed()
      .subscribe(result => {
        if (result) {
          this.getAllAdmins();
        }
      })
  }
}
