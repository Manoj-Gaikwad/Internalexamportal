import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { HttpErrorResponse } from "@angular/common/http";
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { QuestionService } from 'app/layout/services/question.service';
import { SnackBarService } from 'app/layout/services/snackbar.service';
import { numberPattern, QuestionType, quillConfig } from 'app/layout/entities/globalConstants';
import { Question ,QuestionOption} from 'app/layout/entities/models';
import { isNullOrUndefined } from 'util';
import { FSEditorComponent } from 'app/shared/fs-editor-component/fs-editor-component.component';

@Component({
    selector: 'question-add',
    templateUrl: './question-add.component.html',
    styleUrls: ['./question-add.component.scss'],
    providers: [QuestionService],
    standalone: false
})
export class QuestionAddComponent implements OnInit {
  public isSpinner: boolean = false;

  public dialogTitle: string = "Add Question";
  public isEdit: boolean = false;

  public form: FormGroup;
  public question: Question;
  public Id: number;

  public DifficultLevel: Array<any>;
  public subjectTopics: Array<any>;
  public subjects: Array<any>;
  public questionTypes: any;
  public questionTypeEnum = QuestionType;
  public multipleChoice: any[] = ['A', 'B', 'C', 'D'];
  public trueFalseType: any[] = ['True', 'False'];

  public placeholder = "Enter Question Here...";
  public errorMsg: string;
editorInstance: any;
 @ViewChild(FSEditorComponent)
  fseditor!: FSEditorComponent;

  constructor(
    @Inject(MAT_DIALOG_DATA) public parentData: any,
    private questionService: QuestionService,
    private _formBuilder: FormBuilder,
    private snackBar: SnackBarService,
    private dialogRef: MatDialogRef<any>,
  ) {}

  ngOnInit(): void {
    this.initializeForm();
    this.getAllDifficultyLevels();
    this.getAllSubjects();
    this.getQuestionTypes();

    if (this.parentData) {
      this.Id = this.parentData.id;
      this.isEdit = true;
      this.dialogTitle = "Edit Question";
      this.getQuestionDetails();
    }
  }


  initializeForm() {
    this.form = this._formBuilder.group({
      id: [0],
      subjectId: ['', Validators.required],
      subjectTopicId: ['', Validators.required],
      questionTypeId: [QuestionType.MultipleChoice, Validators.required],
      description: ['', [Validators.required, Validators.minLength(15)]],
      questionMark: this._formBuilder.group({
        id: [0],
        rightMark: ['', [Validators.required, Validators.maxLength(2), Validators.pattern(numberPattern)]],
        negativeMark: ['', [Validators.required, Validators.maxLength(2), Validators.pattern(numberPattern)]],
        difficultLevel: ['', Validators.required],
        questionId: [0],
      }),
      questionOption: this._formBuilder.array([]),
    });

    this.setQuestionOptionsFormControl(QuestionType.MultipleChoice);
  }

  setQuestionOptionsFormControl(questionType: number) {
    this.form.setControl('questionOption', this._formBuilder.array([]));

    for (let i = 0; i < this.getNumberOfQuestions(questionType); i++) {
      (<FormArray>this.form.get('questionOption')).push(
        this._formBuilder.group({
          id: 0,
          optionText: ['', Validators.required],
          questionId: [0],
          isAnswer: [(questionType === QuestionType.Subjective) ? true : false],
        })
      );
      
    }
  }

  getOptionTitle(index: number): string {
    switch (this.form.get("questionTypeId").value) {
      case QuestionType.MultipleChoice:
        return this.multipleChoice[index];
      case QuestionType.TrueFalse:
        return this.trueFalseType[index];
      case QuestionType.Subjective:
        return "Answer";
    }
  }

  	changeSelection(position, event) {
		if (this.form.get("questionTypeId").value == QuestionType.TrueFalse) {

			var checked = event.checked;
			if (isNullOrUndefined(checked)) { checked = event.target.checked; }

			let options: FormArray = this.form.get('questionOption') as FormArray;
			for (var i = 0; i < this.getNumberOfQuestions(this.form.get('questionTypeId').value); i++) {
				if (i != position) {
					options.at(i).get('isAnswer').patchValue(false);
				}
			}
    }}

 getNumberOfQuestions(questionType: number): number {
    switch (questionType) {
      case QuestionType.MultipleChoice:
        return 4;
      case QuestionType.TrueFalse:
        return 2;
      case QuestionType.Subjective:
        return 1;
      default:
        return 1;
    }
  }

 getQuestionDetails() {
  this.showSpinner();
  this.questionService.GetQuestionDetails(this.Id).subscribe(
    (res: any) => {
      if (res.success) {
        this.question = res.question;
        this.getSubjectTopics(this.question.subjectId);
        this.setQuestionOptionsFormControl(this.question.questionTypeId);

        // Patch form values (except description)
        this.form.patchValue({
          id: this.question.id,
          subjectId: this.question.subjectId,
          subjectTopicId: this.question.subjectTopicId,
          questionTypeId: this.question.questionTypeId,
          questionMark: this.question.questionMark
        });

        // Set editor content if editor is ready, otherwise it will be set in editorCreated()
        if(this.editorInstance && this.question) {
         this.fseditor.setEditorContent(this.question.description);
        }
        
        // Patch options array
        const optionsArray = this.form.get('questionOption') as FormArray;
        this.question.questionOption.forEach((opt, index) => {
          if (optionsArray.at(index)) optionsArray.at(index).patchValue(opt);
        });

        this.hideSpinner();
      }
    },
    (err: HttpErrorResponse) => {
      this.errorMsg = err.error?.[""]?.[0] || 'Error fetching question details';
      this.hideSpinner();
    }
  );
}


editorCreated(quill: any) {
  this.editorInstance = true;
  // If question data is already loaded, set the content
  if(this.question && this.question.description) {
    this.fseditor.setEditorContent(this.question.description);
  }
}





  getAllDifficultyLevels() {
    this.questionService.getAllDifficultyLevels().subscribe(
       (res: any) => {
        this.DifficultLevel = res;
      },
      (err) => {
        this.errorMsg = err.error[""][0];
      }
    );
  }

  getQuestionTypes() {
    this.questionService.GetQuestionTypes().subscribe(
      (data) => {
        this.questionTypes = data;
      },
      (err: HttpErrorResponse) => {
        console.log(err.message);
      }
    );
  }

  getAllSubjects() {
    this.questionService.GetAllSubjects().subscribe(
      (data) => {
        this.subjects = data;
      },
      (err: HttpErrorResponse) => {
        console.log(err.message);
      }
    );
  }

  getSubjectTopics(subjectId) {
    this.questionService.GetTopic(subjectId).subscribe(
      (data) => {
        this.subjectTopics = data;
      },
      (err: HttpErrorResponse) => {
        console.log(err.message);
      }
    );
  }

  onSubmit() {
    if (!this.validateSelectedOptions()) {
      this.snackBar.error("Select answer");
      return;
    }

    this.question = this.form.value;
  if (!this.isEdit) {
      this.saveQuestion();
    } else {
      this.updateQuestion();
    }
  }

  saveQuestion() {
    this.showSpinner();
    this.questionService.AddQuestion({ question: this.question }).subscribe(
      (res: any) => {
        this.hideSpinner();

        if (res != null) {
          this.snackBar.success('Question Added Successfully');
          this.dialogRef.close(true);
          this.form.reset();
        }
      },
      (err) => {
        this.hideSpinner();
        this.snackBar.error('Error adding question');
      }
    );
  }

  updateQuestion() {
    this.showSpinner();
    this.questionService.UpdateQuestion({ question: this.question }).subscribe(
      (res: any) => {
        this.hideSpinner();
        if (res != null) {
          this.snackBar.success('Question Updated Successfully');
          this.dialogRef.close(true);
        }
      },
      (err) => {
        this.hideSpinner();
        this.snackBar.error('Error updating question');
      }
    );
  }

validateSelectedOptions(): boolean {
    const options: FormArray = this.form.get('questionOption') as FormArray;
    for (let i = 0; i < this.getNumberOfQuestions(this.form.get('questionTypeId').value); i++) {
      if (options.at(i).get('isAnswer').value === true) {
        return true;
      }
    }
    return false;
  }

  showSpinner() { this.isSpinner = true; }
  hideSpinner() { this.isSpinner = false; }
}
