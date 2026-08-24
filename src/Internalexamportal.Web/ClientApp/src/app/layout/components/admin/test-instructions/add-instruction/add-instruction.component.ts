import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { SnackBarService } from 'app/layout/services/snackbar.service';
import { TestService } from 'app/layout/services/test.service';
import { FSEditorComponent } from 'app/shared/fs-editor-component/fs-editor-component.component';

@Component({
    selector: 'cg-add-instruction',
    templateUrl: './add-instruction.component.html',
    styleUrls: ['./add-instruction.component.scss'],
    providers: [TestService],
    standalone: false
})
export class AddInstructionComponent implements OnInit {
  instructionForm!: FormGroup;
  isSpinner: boolean = false;
  title = 'Add Test Instruction';
  placeholder = "Enter Instructions Here...";
  editorInstance: boolean = false;

  @ViewChild(FSEditorComponent)
  fseditor!: FSEditorComponent;

  constructor(
    @Inject(MAT_DIALOG_DATA) public parentData: any,
    private _formBuilder: FormBuilder,
    private testService: TestService,
    private snackBar: SnackBarService,
    private dialogRef: MatDialogRef<any>
  ) {}

  ngOnInit() {
    this.initializeForm();

    if (this.parentData) {
      this.title = 'Edit Test Instruction';
      this.patchFormData();
    }
  }

  initializeForm() {
    this.instructionForm = this._formBuilder.group({
      Id: [0],
      instruction: ['', Validators.required],
      instructionDescripiton: ['', Validators.required],
    });
  }

  patchFormData() {
    this.instructionForm.patchValue({
      Id: this.parentData.id,
      instruction: this.parentData.instruction,
      instructionDescripiton: this.parentData.instructionDescripiton
    });

    // Set content if editor is already initialized
    if (this.editorInstance && this.parentData.instructionDescripiton) {
      this.fseditor.setEditorContent(this.parentData.instructionDescripiton);
    }
  }

  editorCreated(quill: any) {
    this.editorInstance = true;
    // If data already loaded, populate content
    if (this.parentData?.instructionDescripiton) {
      this.fseditor.setEditorContent(this.parentData.instructionDescripiton);
    }
  }

  onSubmit() {
    if (!this.instructionForm.valid) return;

    this.showSpinner();
    const InstructionForm = this.instructionForm.value;

    this.testService.saveInstruction(InstructionForm).subscribe({
      next: (res: any) => {
        this.hideSpinner();
        if (res.success) {
          this.snackBar.success(
            InstructionForm.Id === 0 ? 'Instruction Added Successfully' : 'Instruction Updated Successfully'
          );
          this.dialogRef.close(true);
        } else {
          this.snackBar.error(res.message);
        }
      },
      error: () => {
        this.hideSpinner();
        this.snackBar.error('Something went wrong!');
      }
    });
  }

  showSpinner() { this.isSpinner = true; }
  hideSpinner() { this.isSpinner = false; }
}
