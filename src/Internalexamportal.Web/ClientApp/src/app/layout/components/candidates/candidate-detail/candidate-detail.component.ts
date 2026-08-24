import { Component, OnInit } from '@angular/core';
import { Validators } from "@angular/forms";
import { FuseConfigService } from "@fuse/services/config.service";
import { FormBuilder, FormGroup } from "@angular/forms";
import { CustomHttpService } from "app/layout/services/custom-http.service";
import { MatDialog } from '@angular/material/dialog';
import { Router } from "@angular/router";
import { ActivatedRoute } from "@angular/router";
import { AdminService } from 'app/layout/services/admin.service';
import { SnackBarService } from 'app/layout/services/snackbar.service';

@Component({
    selector: 'cg-candidate-detail',
    templateUrl: './candidate-detail.component.html',
    styleUrls: ['./candidate-detail.component.scss'],
    providers: [AdminService],
    standalone: false
})
export class CandidateDetailComponent implements OnInit {
  candidateDetailForm: FormGroup;
  Id: any;
  public errorMsg = [];
  candidateData: any;
  candidateUpdateData: any;
  userPersonalDetail: any;

  constructor(private _fuseConfigService: FuseConfigService,
    private _formBuilder: FormBuilder,
    private router: Router,
    public dialog: MatDialog,
    private snackBar: SnackBarService,
    private ActiveteRoute: ActivatedRoute,
    private adminService: AdminService) {
  }

  ngOnInit() {

    this.initializeForm();

    this.Id = this.ActiveteRoute.snapshot.queryParams['id'];

    this.GetCandidateDetailsById();
  }

  initializeForm() {
    this.candidateDetailForm = this._formBuilder.group({
      Id: [''],
      email: ['', [Validators.required, Validators.maxLength(50), Validators.email]],
      firstname: ['', [Validators.required]],
      lastname: ['', [Validators.required]],
      username: ['', [Validators.required]],
      gender: ['', [Validators.required]],
      dob: ['', [Validators.required]],
      phonenumber: ['', [Validators.required]],
      address: ['', [Validators.required]],
      state: ['', [Validators.required]],
      city: ['', [Validators.required]],
      zipcode: ['', [Validators.required]]
    });
  }

  GetCandidateDetailsById() {
    this.adminService.GetCandidateDetailsById(this.Id)
      .subscribe((res: any) => {
        this.candidateData = res;
        this.setFormValue();
      },
        err => {
          this.errorMsg = err.error[""][0];
        }
      );
  }

  setFormValue() {

    this.candidateDetailForm.controls['email'].setValue(this.candidateData.email),
      this.candidateDetailForm.controls['username'].setValue(this.candidateData.userName),
      this.candidateDetailForm.controls['firstname'].setValue(this.candidateData.firstName),
      this.candidateDetailForm.controls['lastname'].setValue(this.candidateData.lastName),
      this.candidateDetailForm.controls['phonenumber'].setValue(this.candidateData.phoneNumber),
      this.candidateDetailForm.controls['address'].setValue(this.candidateData.userPersonalDetail.address),
      this.candidateDetailForm.controls['city'].setValue(this.candidateData.userPersonalDetail.city),
      this.candidateDetailForm.controls['state'].setValue(this.candidateData.userPersonalDetail.state),
      this.candidateDetailForm.controls['zipcode'].setValue(this.candidateData.userPersonalDetail.zipCode),
      this.candidateDetailForm.controls['gender'].setValue(this.candidateData.userPersonalDetail.gender),
      this.candidateDetailForm.controls['dob'].setValue(this.candidateData.userPersonalDetail.dob)

  }

  updateCandidateData() {
    //this.candidateDetailForm.forEach(a => a.dateofbirth = a.dateofbirth._d.toISOString());
    this.candidateDetailForm.value.dob = this.candidateDetailForm.value.dob;
    this.candidateUpdateData = this.candidateDetailForm.value;
    this.candidateUpdateData.Id = this.Id;
    this.adminService.UpdateCandidateDetails(this.candidateUpdateData)
      .subscribe(res => {
        this.snackBar.success('Candidate Information Updated Successfully');
        this.router.navigate(['layout/admin/candidate-List']);
      },
        err => {
          this.errorMsg = err.error[""][0];
        }
      );
  }

}
