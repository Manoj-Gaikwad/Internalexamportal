import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
    selector: 'cg-view-instructions',
    templateUrl: './view-instructions.component.html',
    styleUrls: ['./view-instructions.component.scss'],
    standalone: false
})
export class ViewInstructionsComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public description: any, private dialogRef: MatDialogRef<any>) { }

  ngOnInit() {
  }

}
