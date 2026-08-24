import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { CustomHttpService } from 'app/layout/services/custom-http.service';
import { CandidateGroupsComponent } from '../candidate-groups.component';
import { SnackBarService } from 'app/layout/services/snackbar.service';


@Component({
    selector: 'cg-delete-group',
    templateUrl: './delete-group.component.html',
    styleUrls: ['./delete-group.component.scss'],
    standalone: false
})
export class DeleteGroupComponent implements OnInit {

  constructor(
    private snackBar: SnackBarService,
    private http: CustomHttpService,
    @Inject(MAT_DIALOG_DATA) public parentData: any,
     private CandidateGroupsDialogRef: MatDialogRef<CandidateGroupsComponent>,
  ) { }

  ngOnInit(): void {
  }
  deleterow(){

   if(this.parentData.GroupId==null)
   {
     
   }
   else{
    this.http._delete('Candidate/DeleteGroup/'+this.parentData.GroupId).subscribe(data=>{
      this.CandidateGroupsDialogRef.close(true);
       this.snackBar.success('Group deleted Successfully');
    })
   }
   
  }

}
