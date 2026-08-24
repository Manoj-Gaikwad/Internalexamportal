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
import { TestService } from 'app/layout/services/test.service';
import { PermissionEnum } from 'app/layout/entities/permission.enum';
import { AddInstructionComponent } from './add-instruction/add-instruction.component';
import { ViewInstructionsComponent } from './view-instructions/view-instructions.component';
import { DeleteInstructionComponent } from './delete-instruction/delete-instruction.component';

@Component({
    selector: 'cg-test-instructions',
    templateUrl: './test-instructions.component.html',
    styleUrls: ['./test-instructions.component.scss'],
    providers: [TestService],
    standalone: false
})

export class TestInstructionsComponent implements OnInit {

  permissionEnum = PermissionEnum;
  isSpinner: boolean;
  isSearchText: boolean;
  searchText: string;
  public searchTerm = new Subject<string>();
  subject: any;
  public dataSource = new MatTableDataSource<any>();
  arrays: any;

  displayedColumns: string[] = ['id', 'instruction', 'action'];
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(private _testService: TestService, private router: Router, public dialog: MatDialog) { }

  ngOnInit() {
    this.getAllInstructions();

    //search box on change.
    this.searchTerm.pipe(
      debounceTime(100),
      distinctUntilChanged())
      .subscribe(value => {
        this.isSearchText = (!isEmpty(value) && !isNull(value));
        this.applyFilter(value);
      });
  }

  getAllInstructions() {
    this.showSpinner()
    this._testService.GetTestInstructions().subscribe(
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

  AddInstructions() {
    const dialogRef = this.dialog.open(AddInstructionComponent, {
      width: '65%',
      height: '80%',
      panelClass: 'full-height-dialog',
      restoreFocus: false
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.getAllInstructions();
      }
    });
  }

  openDialog(row) {
		const dialogRef = this.dialog.open(DeleteInstructionComponent, {
			disableClose: true,
			panelClass: ['app-no-padding-dialog'],
			data: {
				InstructionId: row.id
			}
		});

		dialogRef.afterClosed()
			.subscribe(result => {
				if (result) {
					this.getAllInstructions();
				}
			})
	}

  EditInstructions(row) {
  const dialogRef = this.dialog.open(AddInstructionComponent, {
    width: '65%',
    height: '80%',
    panelClass: 'full-height-dialog', // Add this
    restoreFocus: false,
    data: row
  });

  dialogRef.afterClosed().subscribe(result => {
    if (result) {
      this.getAllInstructions();
    }
  });
}

  viewInstructions(row:any) {
    const dialogRef = this.dialog.open(ViewInstructionsComponent, {
      width: '75%',
      height:'90%',
      restoreFocus: false,
      data:row.instructionDescripiton
      
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
      }
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

	getRowIndex(i: number): number {
		return (this.paginator.pageIndex == 0 ? i + 1 : 1 + i + this.paginator.pageIndex * this.paginator.pageSize);
  }
  
  clearSearch() {
    this.searchText = '';
    this.isSearchText = false;
    this.paginator.firstPage();
    this.getAllInstructions();
  }

}


