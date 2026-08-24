import { Component, Inject, ViewEncapsulation, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { DomSanitizer } from '@angular/platform-browser';
import { downloadFile} from 'app/layout/entities/globalConstants';
@Component({
    selector: 'app-print-dailog',
    templateUrl: './print-dailog.component.html',
    styleUrls: ['./print-dailog.component.scss'],
    encapsulation: ViewEncapsulation.None,
    standalone: false
})
export class PrintDailogComponent implements OnInit {
  
  templatePdfUrl: any;
  file: Blob;
  fileData:any;
  fileName:string;

  constructor(
    public matDialogRef: MatDialogRef<PrintDailogComponent>,
    private sanitizer: DomSanitizer,
    @Inject(MAT_DIALOG_DATA) private _data: any
  )
  {
    this.fileName = this._data.fileName;
    this.fileData = this._data.fileData;
    this.file = new Blob([this.fileData.body], { type: this.fileData.body.type });
    const fileUrl = URL.createObjectURL(this.file);
    this.templatePdfUrl = this.sanitizer.bypassSecurityTrustResourceUrl(fileUrl);
  }

  ngOnInit() {}

  print(){
    window.frames["docFrame"].focus();
    window.frames["docFrame"].print();
  }

  download(){
    downloadFile(this.fileData, this.fileName)
  }
}

