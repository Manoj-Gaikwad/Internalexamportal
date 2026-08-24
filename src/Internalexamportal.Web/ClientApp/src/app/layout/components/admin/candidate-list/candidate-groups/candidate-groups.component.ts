import { Component, OnInit } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Router } from "@angular/router";
import { HttpErrorResponse } from "@angular/common/http";
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { MatSort } from "@angular/material/sort";
import { ViewChild } from "@angular/core";
import { CustomHttpService } from "app/layout/services/custom-http.service";
import { Input } from "@angular/core";

import { DataServicesService } from "app/layout/services/data-services.service";
import { BlankComponent } from "app/layout/components/blank/blank.component";
import { ActivatedRoute } from "@angular/router";
import { Subject } from "rxjs";
import { debounceTime, distinctUntilChanged } from "rxjs/operators";
import { isEmpty } from "lodash";
import { isNull } from "util";
import { PermissionEnum } from "app/layout/entities/permission.enum";
import { AdminService } from "app/layout/services/admin.service";
import { AddGroupComponent } from './add-group/add-group.component';
import { DeleteGroupComponent } from "./delete-group/delete-group.component";


@Component({
    selector: "cg-candidate-groups",
    templateUrl: "./candidate-groups.component.html",
    styleUrls: ["./candidate-groups.component.scss"],
    providers: [AdminService],
    standalone: false
})
export class CandidateGroupsComponent implements OnInit {
    permissionEnum = PermissionEnum;
    isSpinner: boolean;
    isSearchText: boolean;
    searchText: string;
    public searchTerm = new Subject<string>();
    subject: any;
    public dataSource = new MatTableDataSource<any>();
    arrays: any;

    displayedColumns: string[] = ["SrNo", "name", "Action"];
    @ViewChild(MatSort) sort: MatSort;
    @ViewChild(MatPaginator) paginator: MatPaginator;
    constructor(
        private _adminService: AdminService,
        private router: Router,
        public dialog: MatDialog
    ) {}

    ngOnInit() {
        this.getAllGroups();

        //search box on change.
        this.searchTerm
            .pipe(debounceTime(100), distinctUntilChanged())
            .subscribe((value) => {
                this.isSearchText = !isEmpty(value) && !isNull(value);
                this.applyFilter(value);
            });
    }

    getAllGroups() {
        this.showSpinner();
        this._adminService.getAllGroups().subscribe(
            (data) => {
                this.arrays = data.groups;
                this.dataSource = new MatTableDataSource(this.arrays);
                this.dataSource.sort = this.sort;
                this.dataSource.paginator = this.paginator;
            },
            (err: HttpErrorResponse) => {
                console.log(err.message);
            },
            () => {
                this.hideSpinner();
            }
        );
    }

    AddGroup() {
        const dialogRef = this.dialog.open(AddGroupComponent, {
            width: "400px",
            restoreFocus:false,
            panelClass: ['app-no-padding-dialog'],
        });

        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                this.getAllGroups();
            }
        });
    }

    EditGroup(row) {
        const dialogRef = this.dialog.open(AddGroupComponent, {
            width: "400px",
            restoreFocus:false,
            panelClass: ['app-no-padding-dialog'],
            data: {
                id: row.id,
                name: row.name,
            },
        });

        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                this.getAllGroups();
            }
        });
    }

  DeleteGroup(row) {
    const dialogRef = this.dialog.open(DeleteGroupComponent, {
      disableClose: true,
      panelClass: ['app-no-padding-dialog'],
      data: {
        GroupId: row.id
      }
    });

    dialogRef.afterClosed()
      .subscribe(res => {
        if (res) {
          this.getAllGroups();
        }
      })

  }


    applyFilter(value: string) {
        this.dataSource.filter = value;
        if (this.dataSource.paginator) {
            this.dataSource.paginator.firstPage();
        }
    }

    showSpinner() {
        this.isSpinner = true;
    }

    hideSpinner() {
        this.isSpinner = false;
    }

    clearSearch() {
        this.searchText = "";
        this.isSearchText = false;
        this.paginator.firstPage();
        this.getAllGroups();
    }
    
	getRowIndex(i: number): number {
		return (this.paginator.pageIndex == 0 ? i + 1 : 1 + i + this.paginator.pageIndex * this.paginator.pageSize);
    }
}
