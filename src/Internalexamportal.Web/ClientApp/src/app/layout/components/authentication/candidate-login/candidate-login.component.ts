import { Component, OnInit } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { FuseConfigService } from "@fuse/services/config.service";
import { ViewEncapsulation } from "@angular/core";
import { fuseAnimations } from "@fuse/animations";
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { CustomHttpService } from 'app/layout/services/custom-http.service';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { AccountService } from 'app/layout/services/account.service';
import { SharedService } from 'app/layout/services/shared.service';
import { fuseFullScreenConfig, passwordPattern, emailPattern } from 'app/layout/entities/globalConstants';
import { SnackBarService } from 'app/layout/services/snackbar.service';

@Component({
    selector: 'app-candidate-login',
    templateUrl: './candidate-login.component.html',
    styleUrls: ['./candidate-login.component.scss'],
    encapsulation: ViewEncapsulation.None,
    animations: fuseAnimations,
    standalone: false
})
export class CandidateLoginComponent implements OnInit {

	candidateloginForm: FormGroup;
	public errorMsg = [];
	public isAdmin: boolean = false;
	public showPassword: boolean = false;


	constructor(
		private _fuseConfigService: FuseConfigService,
		private _formBuilder: FormBuilder,
		private accountService: AccountService,
		private sharedService: SharedService,
		private router: Router,
		private snackBar: SnackBarService,
	) {
		this._fuseConfigService.config = fuseFullScreenConfig;
	}


	ngOnInit() {
		this.candidateloginForm = this._formBuilder.group({
			email: ['', [Validators.required, Validators.maxLength(50), Validators.pattern(emailPattern)]],
			password: ['', [Validators.required]],
			rememberMe:[false]
		});
	}

	onSubmit() {
		this.errorMsg = [];
		this.accountService.login(this.candidateloginForm.value)
			.subscribe(res => {
				this.sharedService.setToken(res.tokenResult.token);
				this.sharedService.setUserRolePermissions(res.tokenResult.permissions);
				this.sharedService.setRoles(res.role);
				this.isAdmin = res.isAdmin;
			}, err => {
				this.snackBar.error(err.error['']);
			}, () => {

				if (this.isAdmin) {
					this.router.navigate(['layout/admin/dashboard']);
				} else {
					this.router.navigate(['layout/candidates/my-test']);
				}
			});
	}

	registerHere() {
		this.router.navigate(['./register'])
	}

}
export class candidateModule { }
