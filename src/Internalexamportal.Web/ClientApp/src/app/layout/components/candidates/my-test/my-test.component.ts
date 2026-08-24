import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { CandidateService } from 'app/layout/services/candidate.service';

@Component({
    selector: 'cg-my-test',
    templateUrl: './my-test.component.html',
    styleUrls: ['./my-test.component.scss'],
    providers: [CandidateService],
    standalone: false
})
export class MyTestComponent implements OnInit {

  public testCount: number;
  public isSpinner: boolean = false;


  constructor(private _candidateService: CandidateService) { }

  ngOnInit() {
    this.getCandidateTests();
  }

  getCandidateTests() {
    this.showSpinner();
    this._candidateService.GetCandidateTestsCount()
      .subscribe(res => {
        this.hideSpinner();
        this.testCount = res.count;
      })
  }

  currentexam() {

    var url = window.location.origin + window.location.pathname + '#/layout/candidates/activation';

    var myWindow = window.open(url, "", "width=2050,height=1000,toolbar=yes,titlebar=yes,location=yes,menubar=yes");


    var timer = setInterval(() => {
      if (myWindow.closed) {
        clearInterval(timer);
        this.getCandidateTests();
      }
    }, 1000);

  }

	showSpinner() { this.isSpinner = true }

	hideSpinner() { this.isSpinner = false }

}
