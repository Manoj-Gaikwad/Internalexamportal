import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Router } from "@angular/router";
import { HttpErrorResponse } from "@angular/common/http";
import { MatTableDataSource } from "@angular/material/table";
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from "@angular/material/paginator";
import { DeleteCandidateComponent } from "app/layout/components/candidates/delete-candidate/delete-candidate.component";
import { MatDialog } from "@angular/material/dialog";
import { AdminService } from 'app/layout/services/admin.service';
import { CandidatePaginate } from 'app/layout/entities/paginate';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { isEmpty } from 'lodash';
import { isNull } from 'util';
import { Papa, ParseResult, PapaParseParser } from 'ngx-papaparse';
import { MatSelectChange } from '@angular/material/select';
import { maxCSVFileSizeInMegaBytes, candidateImportCSVFilePath, exportCSVOptions } from 'app/layout/entities/globalConstants';
import { CandidateModel } from 'app/layout/entities/models';
import { ImportCandidateListComponent } from '../import-candidate-list/import-candidate-list.component';
import { PermissionEnum } from 'app/layout/entities/permission.enum';
import { ExportToCsv } from 'export-to-csv';
import  moment from 'moment';
import { SnackBarService } from 'app/layout/services/snackbar.service';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { AddCandidateComponent } from './add-candidate/add-candidate.component';

export interface Candidate {
	SrNo: number;
	CandidateName: string;
	Email: string;
	MobileNo: number;
}

var arr: Candidate[];
@Component({
    selector: 'cg-candidate-list',
    templateUrl: './candidate-list.component.html',
    styleUrls: ['./candidate-list.component.scss'],
    providers: [AdminService],
    animations: [
        trigger('detailExpand', [
            state('collapsed', style({ height: '0px', minHeight: '0', display: 'none' })),
            state('expanded', style({ height: '*' })),
            transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
        ]),
    ],
    standalone: false
})
export class CandidateListComponent implements OnInit {

	permissionEnum = PermissionEnum;

	Id: any;
	_unsubscribeAll: any;
	public dataSource = new MatTableDataSource<Candidate>(arr);
	arrays: any;
	candidateData: any;
	public getName: string;
	public errorMsg = [];
	public importFileUrl: string;
	public expandedElement: any;

	public groups: Array<any>;
	public isSearchText: boolean;
	public searchText: string;
	public searchTerm = new Subject<string>();
	public paginate = new CandidatePaginate();
	public isSpinner: boolean = false;

	displayedColumns: string[] = ['toggle', 'rollNumber', 'name', 'email', 'phoneNumber', 'group', 'date', 'status', 'action'];
	@ViewChild(MatSort) sort: MatSort;
	@ViewChild(MatPaginator) paginator: MatPaginator;

	@ViewChild('importDialog') importDialog: TemplateRef<any>;

	constructor(private router: Router, public dialog: MatDialog,
		private adminService: AdminService, private papa: Papa,
		private snackBar: SnackBarService) { }

	ngOnInit() {
		this.getAllCandidates();
		this.getAllGroups();
		this.importFileUrl = window.location.origin + window.location.pathname + candidateImportCSVFilePath;

		//search box on change.
		this.searchTerm.pipe(
			debounceTime(500),
			distinctUntilChanged())
			.subscribe(value => {
				this.isSearchText = (!isEmpty(value) && !isNull(value));
				this.applyFilter(value);
			});
	}

	getAllGroups() {
		this.showSpinner()
		this.adminService.getAllGroups()
			.subscribe((res: any) => {
				this.groups = res.groups;
				this.hideSpinner();
			},
				err => {
					this.hideSpinner();
					console.error(err);
				}
			);
	}

	getAllCandidates() {
		this.showSpinner()
		this.adminService.GetAllCandidates(this.paginate).subscribe(
			(data: any) => {
				this.arrays = data.candidates;
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

	onGroupSelectionChange(event: MatSelectChange) {
		this.paginate.groups = event.value;
		this.getAllCandidates();
	}
	sortChange(event: any) {
		var column = event.active
		this.paginate.sortingColumn = column;
		this.paginate.sortingDirection =
			event.direction ? event.direction : this.paginate.sortingDirection == 'asc' ? 'desc' : 'asc';
		this.paginator.firstPage();
		this.getAllCandidates();
	}

	nextOrPreviousPage(event) {
		if (this.paginate.pageSize != event.pageSize) {
			this.paginate.pageNumber = 1;
			this.paginator.firstPage();
		} else {
			this.paginate.pageNumber = event.pageIndex + 1;
		}
		this.paginate.pageSize = event.pageSize;
		this.getAllCandidates();
	}

	showSpinner() { this.isSpinner = true }

	hideSpinner() { this.isSpinner = false }


	clearSearch() {
		this.searchText = '';
		this.paginate.searchTerm = null;
		this.isSearchText = false;
		this.paginator.firstPage();
		this.getAllCandidates();
	}

	applyFilter(value: string) {
		this.paginate.pageNumber = 1;
		this.paginate.searchTerm = value.trim().toLowerCase();
		this.paginator.firstPage();
		this.getAllCandidates();
	}

	getRowIndex(i: number): number {
		return (this.paginator.pageIndex == 0 ? i + 1 : 1 + i + this.paginator.pageIndex * this.paginator.pageSize);
	}

	toggleActiveStatus(user) {
		this.adminService.changeUserStatus(user.id)
			.subscribe((res: any) => {
				this.snackBar.success(`Candidate made ${(user.isActive ? 'Inactive' : 'Active')} Successfully.`);
				this.getAllCandidates();
			}, () => { }
			)

	}

	DeleteCandidate(row) {

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
					this.getAllCandidates();
				}
			})
	}

	openAddCandidateDialog() {

		const dialogRef = this.dialog.open(AddCandidateComponent, {
			disableClose: true,
			panelClass: ['app-no-padding-dialog'],
			restoreFocus: false
		});

		dialogRef.afterClosed()
			.subscribe(result => {
				if (result) {
					this.getAllCandidates();
				}
			})
	}

	EditCandidate(row) {

		const dialogRef = this.dialog.open(AddCandidateComponent, {
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
					this.getAllCandidates();
				}
			})
	}

	onFileInput(event) {

		var input = event.target;

		if (input.files.length > 0) {

			var csvFile = input.files[0];

			let FileSize = csvFile.size / 1024 / 1024;

			let filename = csvFile.name;

			if (filename.includes('.csv') && FileSize < maxCSVFileSizeInMegaBytes) {

				var allKeyPresent: boolean = false;

				let options = {
					complete: (results: ParseResult) => {

						if (results.data != null && results.data.length > 0) {
							this.handleCSVOutput(results.data);
						} else {
							this.snackBar.error('Empty csv or parse error.');
							this.hideSpinner();
						}
					},
					skipEmptyLines: true,
					header: true
				}

				this.showSpinner();

				this.papa.parse(csvFile, options);

			} else {
				let message = (FileSize < maxCSVFileSizeInMegaBytes) ? 'Invalid file type' : 'File size should be less than ' + maxCSVFileSizeInMegaBytes + 'MB';
				this.snackBar.error(message);
			}
		}
	}

	handleCSVOutput(output: Array<CandidateModel>) {

		if (this.checkCSVColumType(output[0])) {

			const dialogRef = this.dialog.open(ImportCandidateListComponent, {
				disableClose: true,
				restoreFocus: false,
				position: {
					top: '50px'
				},
				data: {
					candidates: output,
					groups: this.groups
				}
			});

			dialogRef.afterClosed()
				.subscribe(result => {
					if (result) {
						this.getAllCandidates();
					}
				})
		}
		this.hideSpinner();
	}

	checkCSVColumType(object): boolean {
		if ('FirstName' in object && 'LastName' in object &&
			'Email' in object && 'Phone' in object &&
			'Gender' in object && 'DateOfBirth' in object) {
			return true;
		} else {
			this.snackBar.error('Required fields not found in selected file.');
			return false;
		}
	}

	openImportDialog() {
		const dialogRef = this.dialog.open(this.importDialog, {
			width: '400px',
			panelClass: ['app-no-padding-dialog'],
			disableClose: true,
			restoreFocus: false
		});

		dialogRef.afterClosed().subscribe(result => {

		})
	}

	openExportDialog(exportDialog) {
		const dialogRef = this.dialog.open(exportDialog, {
			width: '400px',
			panelClass: ['app-no-padding-dialog'],
			disableClose: true,
			restoreFocus: false
		});

		dialogRef.afterClosed().subscribe(result => {
			if (result) {
				this.getExportData();
			}
		})
	}

	getExportData() {
		this.showSpinner()
		this.adminService.getExportCandidateData(this.paginate).subscribe(
			(data: any) => {

				const csvExporter = new ExportToCsv(exportCSVOptions);

				csvExporter.options.filename = exportCSVOptions.filename + moment().format('DD_MM_YYYY_HH_mm');

				csvExporter.generateCsv(data.candidates);
			},
			(err: HttpErrorResponse) => {
				console.log(err.message);
			}, () => {
				this.hideSpinner();
			}
		);
	}
}
