import { Component, OnInit } from '@angular/core';
import { CustomHttpService } from 'app/layout/services/custom-http.service';
import { Router } from "@angular/router";
import { SharedService } from "app/layout/services/shared.service";
import { HttpClient } from "@angular/common/http";
import { HttpErrorResponse } from "@angular/common/http";
import { AdminService } from 'app/layout/services/admin.service';
import { PermissionEnum } from 'app/layout/entities/permission.enum';
import { SnackBarService } from 'app/layout/services/snackbar.service';

@Component({
    selector: 'cg-admin',
    templateUrl: './admin.component.html',
    styleUrls: ['./admin.component.scss'],
    providers: [AdminService],
    standalone: false
})
export class AdminComponent implements OnInit {

	permissionEnum = PermissionEnum
	subjectCount: number;
	testCount: number;
	candidateCount: number;
	questionCount: number;
	isSpinner: boolean;

	constructor(private router: Router, private adminService: AdminService,
				private _snackbar:SnackBarService) { }

	ngOnInit(): void {
		this.getDashboardData();
	}

	getDashboardData() {
		this.isSpinner = true;
		this.adminService.getDashboardData()
			.subscribe(res => {
				this.isSpinner = false;
				this.subjectCount = res.subjectCount;
				this.candidateCount = res.candidateCount;
				this.questionCount = res.questionsCount;
				this.testCount = res.testCount;

			}, (error) => {
				console.error(error);
			});
	}

	AddCandidate() {
		this.router.navigate(['layout/admin/candidate-list']);
	}
	AddQuestion() {
		this.router.navigate(['layout/admin/question']);
	}
	AddTest() {
		this.router.navigate(['layout/admin/tests']);
	}

	ViewQuestions() {
		this.router.navigate(['layout/admin/question']);
	}




}
