import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'cg-import-question',
    templateUrl: './import-question.component.html',
    styleUrls: ['./import-question.component.scss'],
    standalone: false
})
export class ImportQuestionComponent implements OnInit {
    selected: any;
    pathArr: string[];
  constructor() { }

  ngOnInit() {
  }

}
