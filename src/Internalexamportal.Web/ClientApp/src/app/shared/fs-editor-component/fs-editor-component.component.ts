import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { QuillEditorComponent as NgxQuillEditorComponent } from 'ngx-quill';
import { quillConfig } from 'app/layout/entities/globalConstants';
import Editor from 'quill/core/editor';

@Component({
    selector: 'fs-editor',
    templateUrl: './fs-editor-component.component.html',
    styleUrls: ['./fs-editor-component.component.scss'],
    standalone: false
})
export class FSEditorComponent implements OnInit {

  @Input() label: string = 'Description';
  @Input() placeholder: string = 'Type here...';
  @Input() control!: FormControl;
  @Input() initialValue: string = '';
  @Output() editorCreated = new EventEmitter<any>();

  @ViewChild('editor') editor!: NgxQuillEditorComponent;

  public config = quillConfig;
  public editorInstance: any;

  ngOnInit(): void {
    if (this.initialValue && this.control) {
      this.control.setValue(this.initialValue);
    }
  }

  /** Called when Quill editor is initialized */
  onEditorCreated(quill: any) {
    this.editorInstance = quill;
    this.editorCreated.emit(quill);

    // Initialize editor with initial content
    if (this.initialValue) {
      this.setEditorContent(this.initialValue);
    }
  }

  /** Dynamically sets HTML/text content in editor */
  setEditorContent(content: string) {
    // Update form control first
    this.control?.setValue(content);

    // Update the Quill editor content directly
    if (this.editorInstance && this.editor) {
      setTimeout(() => {
        if (this.editor.quillEditor) {
          this.editor.quillEditor.setText('');
          this.editor.quillEditor.clipboard.dangerouslyPasteHTML(content);
        }
      });
    }
  }
}
