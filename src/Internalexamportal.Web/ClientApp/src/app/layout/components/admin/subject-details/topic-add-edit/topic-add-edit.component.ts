import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { QuestionService } from 'app/layout/services/question.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SnackBarService } from 'app/layout/services/snackbar.service';

@Component({
    selector: 'cg-topic-add-edit',
    templateUrl: './topic-add-edit.component.html',
    styleUrls: ['./topic-add-edit.component.scss'],
    providers: [QuestionService],
    standalone: false
})
export class TopicAddEditComponent implements OnInit {

	public isSpinner: boolean = false;

	TopicForm: FormGroup;
	subjectId: number;
	id: number = 0;
	topic: string;
	isEdit: boolean = false;
	title: string = "Add Topic"

	constructor(private _questionService: QuestionService, private _formBuilder: FormBuilder,
		private dialogRef: MatDialogRef<any>, @Inject(MAT_DIALOG_DATA) public parentData: any,
		private snackBar: SnackBarService) { }

	ngOnInit() {
		this.subjectId = this.parentData.subjectId;
		if (this.parentData.id != null) {
			this.id = this.parentData.id;
			this.topic = this.parentData.topic;
			this.isEdit = true;
			this.title = "Edit Topic";
		}
		this.buildForm();
	}

	buildForm() {
		this.TopicForm = this._formBuilder.group({
			Id: [this.id],
			Topic: [this.topic, Validators.required],
			SubjectId: [this.subjectId],
		});
	}

	onSubmit() {
		var topicForm = this.TopicForm.value;
		if (this.isEdit) {
			this.editTopic(topicForm);
		} else {
			this.addTopic(topicForm)
		}
	}

	addTopic(subjectTopic) {
		this.showSpinner();
		this._questionService.AddTopic(subjectTopic)
			.subscribe(
				(res: any) => {
					this.hideSpinner();
					if (res.success) {
						this.snackBar.success('Subject Topic Added Successfully');
						this.dialogRef.close(true);
					} else {
						this.snackBar.error(res.message);
					}
				},
				err => {
					this.hideSpinner();
					console.error(err);

				}
			);
	}


	editTopic(subjectTopic) {
		this.showSpinner();
		this._questionService.EditTopic(subjectTopic)
			.subscribe(
				(res: any) => {
					this.hideSpinner();
					if (res.success) {
						this.snackBar.success('Subject Topic Updated Successfully');
						this.dialogRef.close(true);
					} else {
						this.snackBar.error(res.message);
					}
				},
				err => {
					this.hideSpinner();
					console.error(err);
				}
			);
	}

	showSpinner() { this.isSpinner = true }

	hideSpinner() { this.isSpinner = false }
}
