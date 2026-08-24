import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { maxTestWarningCount } from 'app/layout/entities/globalConstants';

@Component({
    selector: 'cg-warning-dialog',
    templateUrl: './warning-dialog.component.html',
    styleUrls: ['./warning-dialog.component.scss'],
    standalone: false
})
export class WarningDialogComponent implements OnInit {

  public count: number = 0;
  public maxCount:number = maxTestWarningCount;

  constructor(@Inject(MAT_DIALOG_DATA) public parentData: any, private dialogRef: MatDialogRef<any>) { }

  ngOnInit() {
    console.log(this.parentData);
    
    if (this.parentData) {
      this.count = this.parentData.count;
    }
  }

}
