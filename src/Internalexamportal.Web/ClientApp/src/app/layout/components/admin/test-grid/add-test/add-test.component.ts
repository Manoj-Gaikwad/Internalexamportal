import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArray, FormControl } from '@angular/forms';
import { SnackBarService } from 'app/layout/services/snackbar.service';
import { TestService } from 'app/layout/services/test.service';
import { QuestionService } from 'app/layout/services/question.service';
import { HttpErrorResponse } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { MatStepper } from '@angular/material/stepper';
import { ActivatedRoute, Router } from '@angular/router';
import { emailPattern, TestSetting } from 'app/layout/entities/globalConstants';
import { AddInstructionComponent } from '../../test-instructions/add-instruction/add-instruction.component';
import moment from 'moment';
import { Subscription } from 'rxjs';

@Component({
    selector: 'cg-add-test',
    templateUrl: './add-test.component.html',
    styleUrls: ['./add-test.component.scss'],
    providers: [TestService, QuestionService],
    standalone: false
})
export class AddTestComponent implements OnInit {

	public title = "Add Test Details";
	public isSpinner: boolean = false;
	public form: FormGroup;
	public isEdit: boolean = false;
	public testId: number = 0;
	public minDate: Date;
	public minTime: string = "12:00 am"
	public test: any;
	public testSettings: Array<any>;

	public testTypes: Array<any>;
	public instructions: Array<any>;
	public difficultyLevels: Array<any>;
	public activationTypes: Array<any>;
	subscriptions: Subscription[] = [];

	@ViewChild('stepper') private myStepper: MatStepper;

	constructor(private _formBuilder: FormBuilder, private snackBar: SnackBarService,
		private _testService: TestService, private _questionService: QuestionService,
		public dialog: MatDialog, private route: ActivatedRoute,
		public router: Router) { }

	ngOnInit() {
		let id = this.route.snapshot.paramMap.get('testId');
		if (id) {
			this.testId = parseInt(id);
			this.isEdit = true;
			this.title = "Edit Test Details";
			this.getTestDetails();
		} else {
			this.minDate = new Date();
			this.GetTestSettings();
		}

		this.getAllDifficultyLevels();
		this.getQuestionTypes();
		this.getTestInstructions();
		this.getActivationTypes();
		this.buildForms();
	}

	buildForms() {

		this.form = this._formBuilder.group({
			testDetails: this._formBuilder.group({
				Id: [this.testId],
				testName: ['', Validators.required, this.isTestNameUnique.bind(this)],
				testInstructionId: ['', Validators.required],
				testTypeId: ['', Validators.required],
				duration: ['', [Validators.required, Validators.min(1), Validators.max(600)]],
				difficultLevelId: ['', Validators.required],
				totalQuestion: ['', [Validators.required, Validators.min(1), Validators.max(250)]],
				totalMark: ['', [Validators.required, Validators.min(1), Validators.max(5000)]],
				percentage: ['', [Validators.required, Validators.min(1), Validators.max(100)]]
			}),
			testSettings: this._formBuilder.array([]),
			testPublish: this._formBuilder.group({
				id: [0],
				testId: [this.testId],
				startDate: [this.minDate, Validators.required],
				endDate: [this.minDate, Validators.required],
				startTime: ['', Validators.required],
				endTime: ['', Validators.required],
			}),
			testActivation: this._formBuilder.group({
				id: [0],
				testId: [this.testId],
				activationTypeId: [1, Validators.required],
			})
		});
		this.setActivationType(1);

		this.form.get('testActivation.activationTypeId').valueChanges.subscribe(type => {
			this.setActivationType(type);
		})

		this.form.get('testPublish').valueChanges.subscribe(type => {
			this.getMinEndTime();
		})
	}

	setActivationType(type: number) {
		if (type == 1) {
			(this.form.get('testActivation') as FormGroup).addControl('commonCode',
				this._formBuilder.group({
					id: 0,
					activationCodeId: 0,
					commonTestCode: ['', Validators.minLength(5)],
				}));
			(this.form.get('testActivation') as FormGroup).removeControl('accessCode');
		} else if (type == 2) {
			(this.form.get('testActivation') as FormGroup).addControl('accessCode',
				this._formBuilder.group({
					id: 0,
					activationCodeId: 0,
					email: ['', [Validators.required, Validators.pattern(emailPattern)]],
				}));
			(this.form.get('testActivation') as FormGroup).removeControl('commonCode');
		}
		return type;
	}

	isTestNameUnique(control: FormControl) {
		const q = new Promise((resolve, reject) => {
			setTimeout(() => {
				this.unsubscribeSubscriptions();
				let subscription = this._testService.CheckIfTestNameExists({ id: this.testId, testName: control.value })
					.subscribe(res => {
						if (res.isExists) {
							resolve({ 'isUnique': true });
						} else {
							resolve(null);
						}
					});
				this.subscriptions.push(subscription);
			}, 0);
		});
		return q;
	}


	unsubscribeSubscriptions() {
		if (this.subscriptions && this.subscriptions.length > 0) {
			this.subscriptions.forEach(subscription => { subscription.unsubscribe() });
		}
	}

	ngOnDestroy() {
		this.unsubscribeSubscriptions();
	}

	getTestDetails() {
		this.showSpinner();
		this._testService.GetTestDetails(this.testId)
			.subscribe((res: any) => {
				this.test = res;
				this.minDate = res.testPublish.startDate;
				this.form.patchValue(res);
				this.setTestSettingFormControl(res.testSettings);
				this.hideSpinner();
			});
	}

	GetTestSettings() {
		this._testService.GetTestSettingTypes(this.testId).subscribe(
			data => {
				this.setTestSettingFormControl(data.testSettings);
			},
			(err: HttpErrorResponse) => {
				console.log(err.message);
			}
		);
	}

	setTestSettingFormControl(testSettings) {
		this.testSettings = testSettings;
		testSettings.forEach((setting, index) => {
			this.testSettings[index].showBtn = (this.getTestSettingDescription(index).length > 85);
			(<FormArray>this.form.get('testSettings')).push(this._formBuilder.group({
				id: setting.id,
				isChecked: setting.isChecked,
				type: setting.type
			}))
		});
	}

	getTestSettingName(index: number): string {
		return this.form.get('testSettings').value[index].type;
	}

	getTestSettingDescription(index: number): string {
		switch (index + 1) {
			case TestSetting.Shuffling:
				return "Enable this setting to shuffle the sequence of the questions in test";
			case TestSetting.MultipleAttempts:
				return "Enable this setting to allow multiple attempts for a test.";
			case TestSetting.DisplayResult:
				return "Enable this setting to display the result of test to the candidate.";
			case TestSetting.WindowMinimiseWarning:
				return "Enable this setting to display warning if the candidate tries to move out of test window. This setting also auto submits the test if multiple warnings are given to user.";
			case TestSetting.DisplayAnswerSheet:
				return "Enable this setting to display the answer sheet of test to the candidate once result is published.";
			default:
				return "Enable/Disable";
		}
	}

	getTestInstructions() {
		this._testService.GetTestInstructions()
			.subscribe(
				(res: any) => {
					this.instructions = res;
				},
				(err: HttpErrorResponse) => {
					console.error(err.message);
				}
			);
	}

	getAllDifficultyLevels() {
		this._questionService.getAllDifficultyLevels()
			.subscribe(
				(res: any) => {
					this.difficultyLevels = res;
				},
				(err: HttpErrorResponse) => {
					console.error(err.message);
				}
			);
	}

	getQuestionTypes() {
		this._testService.GetTestTypes().subscribe(
			data => {
				this.testTypes = data;
			},
			(err: HttpErrorResponse) => {
				console.log(err.message);
			}
		);
	}

	getActivationTypes() {
		this._testService.GetTestCodeTypes()
			.subscribe(
				data => {
					this.activationTypes = data;
				},
				(err: HttpErrorResponse) => {
					console.log(err.message);
				}
			);
	}

	openAddInstructionDialog() {

		const dialogRef = this.dialog.open(AddInstructionComponent, {
			disableClose: true,
			width: '65%'
		});

		//get new added test instructions into the dropdown after the dialog is closed
		dialogRef.afterClosed().subscribe(res => {
			if (res) {
				this.getTestInstructions();
			}
		});
	}

	appendTimeToDate() {
  const startDate = this.form.get('testPublish.startDate')?.value;
  const startTime = this.form.get('testPublish.startTime')?.value;
  const endDate = this.form.get('testPublish.endDate')?.value;
  const endTime = this.form.get('testPublish.endTime')?.value;

  if (startDate && startTime) {
    const start = moment(startDate)
      .set({
        hour: moment(startTime, ['h:mm A']).hour(),
        minute: moment(startTime, ['h:mm A']).minute()
      })
      .utc(true)
      .toISOString();

    this.form.get('testPublish.startDate')?.patchValue(start);
  }

  if (endDate && endTime) {
    const end = moment(endDate)
      .set({
        hour: moment(endTime, ['h:mm A']).hour(),
        minute: moment(endTime, ['h:mm A']).minute()
      })
      .utc(true)
      .toISOString();

    this.form.get('testPublish.endDate')?.patchValue(end);
  }

}

	onSubmit() {
		this.showSpinner();
		this.appendTimeToDate();
		this._testService.UpdateTestDetails(this.form.value)
			.subscribe(res => {
				this.hideSpinner();
				if ((this.testId != 0 || this.isEdit) && res.success) {
					this.router.navigate(['layout/admin/tests']);
				} else {
					this.testId = res.testId;
					this.form.get('testDetails.Id').patchValue(res.testId);
					this.form.get('testPublish.id').patchValue(res.publishId);
					this.form.get('testPublish.testId').patchValue(res.testId);
					this.form.get('testActivation.id').patchValue(res.activationId);
					this.form.get('testActivation.testId').patchValue(res.testId);
				}
				if (!res.success) {
					this.snackBar.error(res.message);
				} else {
					if (!this.isEdit)
						this.form.get('testActivation.commonCode.commonTestCode').patchValue(res.code);
					this.snackBar.success("Test Updated Successfully");
				}
			}, err => {
				console.error(err);
			})
	}

	nextStep() {
		if (this.myStepper.selectedIndex == 2 && this.testId == 0) {
			this.onSubmit();
		}
		this.myStepper.next();
	}

	testDetails(): any {
		return {
			totalQuestion: this.form.get('testDetails.totalQuestion').value,
			totalMark: this.form.get('testDetails.totalMark').value,
			testTypeId: this.form.get('testDetails.testTypeId').value
		}
	}

	getMinEndTime() {
		const startDate = this.form.get('testPublish.startDate').value;
		const sd = moment(startDate).format('L');
		const endDate = this.form.get('testPublish.endDate').value;
		const ed = moment(endDate).format('L');
		const startTime = this.form.get("testPublish.startTime").value;
		if (sd == ed && startTime != '') {
			this.minTime = startTime;
		} else {
			this.minTime = '12:00 am';
		}
	}

	showSpinner() { this.isSpinner = true }

	hideSpinner() { this.isSpinner = false }
}
