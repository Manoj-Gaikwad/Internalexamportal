import { Component, OnInit } from '@angular/core';
import { CustomHttpService } from "app/layout/services/custom-http.service";
import { Router } from "@angular/router";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { Inject } from "@angular/core";
import { SnackBarService } from 'app/layout/services/snackbar.service';

@Component({
    selector: 'cg-delete-instruction',
    templateUrl: './delete-instruction.component.html',
    styleUrls: ['./delete-instruction.component.scss'],
    standalone: false
})
export class DeleteInstructionComponent implements OnInit {
Id: any;
  errorMsg: any;

  constructor(private router: Router,
              private http: CustomHttpService,
              @Inject(MAT_DIALOG_DATA) public parentData: any,
              private dialogRef: MatDialogRef<any>,
              private snackBar: SnackBarService) { }

  ngOnInit() {
  }
  deleterow1(){
   this.Id=this.parentData.InstructionId;
   this.http._delete('Test/DeleteInstruction/' + this.Id)  
      .subscribe(
      (res: any) => {
           this.snackBar.success('Instruction deleted Successfully');
            this.dialogRef.close(true);
      },
      err => {
        this.errorMsg = err.error[""][0];
      }
      );
}

}
