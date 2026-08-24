import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { CustomHttpService } from "app/layout/services/custom-http.service";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { Inject } from "@angular/core";
import { SnackBarService } from 'app/layout/services/snackbar.service';
import { ClientService } from 'app/layout/services/client.service';
import { FormGroup } from '@angular/forms';


@Component({
    selector: 'cg-delete-client',
    templateUrl: './delete-client.component.html',
    styleUrls: ['./delete-client.component.scss'],
    providers: [ClientService],
    standalone: false
})
export class DeleteClientComponent implements OnInit {
  Id:any;
  public errorMsg = [];
form : FormGroup;
  constructor(private router: Router,
    private http: CustomHttpService,
    @Inject(MAT_DIALOG_DATA) public parentData: any,
    private dialogRef: MatDialogRef<any>,
    private snackBar: SnackBarService,
    private _clientService: ClientService) { }

  ngOnInit() {
  }

  onClick() {

    this.Id=this.parentData.InstructionId;
    // this.http._delete('client/DeleteClient/'+{Id})  
    //    .subscribe(
    //    (res: any) => {
    //         this.snackBar.success('Client deleted Successfully');
    //          this.dialogRef.close(true);
    //    },
    //    err => {
    //     console.log("hi");
    //     //  this.errorMsg = err.error[""][0];
    //    }
    //    );

       this._clientService.DeleteClient(this.Id)
       .subscribe(() => {
        this.snackBar.success('Client deleted Successfully');
        this.dialogRef.close(true);
       },
       err => {
         console.log("hi");
        this.errorMsg = err.error[""][0];
       }
       )
       
}
}
