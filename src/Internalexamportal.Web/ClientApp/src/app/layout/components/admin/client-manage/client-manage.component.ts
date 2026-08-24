import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { Client } from 'app/layout/entities/models';
import { Subject } from 'rxjs';
import { Paginate } from 'app/layout/entities/paginate';
import { Router } from '@angular/router';
import { AdminService } from 'app/layout/services/admin.service';
import { Papa } from 'ngx-papaparse';
import { SnackBarService } from 'app/layout/services/snackbar.service';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { isNull } from 'util';
import { HttpErrorResponse } from '@angular/common/http';
import { AddCandidateComponent } from '../candidate-list/add-candidate/add-candidate.component';
import { isEmpty } from 'lodash';
import { ClientService } from 'app/layout/services/client.service';
import { AddEditClientComponent } from './add-edit-client/add-edit-client.component';
import { SharedService } from 'app/layout/services/shared.service';
import { Roles } from 'app/layout/entities/globalConstants';
import { DeleteClientComponent } from './delete-client/delete-client.component';

@Component({
    selector: 'cg-client-manage',
    templateUrl: './client-manage.component.html',
    styleUrls: ['./client-manage.component.scss'],
    providers: [ClientService],
    standalone: false
})
export class ClientManageComponent implements OnInit {

  _unsubscribeAll: any;
  public dataSource = new MatTableDataSource<Client>();
  arrays: any;
  public errorMsg = [];

  public isSearchText: boolean;
  public searchText: string;
  public searchTerm = new Subject<string>();
  public paginate = new Paginate();
  public isSpinner: boolean = false;

  displayedColumns: string[] = ['id', 'name', 'email', 'phone', 'action'];
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(private router: Router, public dialog: MatDialog,
    private _clientService: ClientService, private snackBar: SnackBarService,
    private _sharedService: SharedService) { }

  ngOnInit() {

    if (!this._sharedService.checkForRole(Roles.SuperAdmin)) {
      this.router.navigate(['']);
    }

    this.getAllClients();

    //search box on change.
    this.searchTerm.pipe(
      debounceTime(500),
      distinctUntilChanged())
      .subscribe(value => {
        this.isSearchText = (!isEmpty(value) && !isNull(value));
        this.applyFilter(value);
      });
  }

  getAllClients() {
    this.showSpinner()
    this._clientService.GetClientsGrid(this.paginate).subscribe(
      (data: any) => {
        this.arrays = data.clients;
        this.paginate.totalRecords = data.totalCount;
        this.dataSource = new MatTableDataSource(this.arrays);
      },
      (err: HttpErrorResponse) => {
        console.log(err.message);
        this.hideSpinner();
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
    this.getAllClients();
  }

  nextOrPreviousPage(event) {
    if (this.paginate.pageSize != event.pageSize) {
      this.paginate.pageNumber = 1;
      this.paginator.firstPage();
    } else {
      this.paginate.pageNumber = event.pageIndex + 1;
    }
    this.paginate.pageSize = event.pageSize;
    this.getAllClients();
  }

  showSpinner() { this.isSpinner = true }

  hideSpinner() { this.isSpinner = false }


  clearSearch() {
    this.searchText = '';
    this.paginate.searchTerm = null;
    this.isSearchText = false;
    this.paginator.firstPage();
    this.getAllClients();
  }

  applyFilter(value: string) {
    this.paginate.pageNumber = 1;
    this.paginate.searchTerm = value.trim().toLowerCase();
    this.paginator.firstPage();
    this.getAllClients();
  }


  openAddClientDialog() {

    const dialogRef = this.dialog.open(AddEditClientComponent, {
      disableClose: true,
      panelClass: ['app-no-padding-dialog'],
      restoreFocus: false
    });

    dialogRef.afterClosed()
      .subscribe(result => {
        if (result) {
          this.getAllClients();
        }
      })
  }

  openEditClientDialog(row) {

    const dialogRef = this.dialog.open(AddEditClientComponent, {
      disableClose: true,
      restoreFocus: false,
      panelClass: ['app-no-padding-dialog'],
      data: {
        id: row.id
      }
    });

    
    dialogRef.afterClosed()
      .subscribe(result => {
        if (result) {
          this.getAllClients();
        }
      })
  }

  deleteClientDialog(row) {
		const dialogRef = this.dialog.open(DeleteClientComponent, {
			disableClose: true,
			panelClass: ['app-no-padding-dialog'],
			data: {
				InstructionId: row.id
			}
		});

		dialogRef.afterClosed()
			.subscribe(result => {
				if (result) {
					this.getAllClients();
				}
			})
	}

}

