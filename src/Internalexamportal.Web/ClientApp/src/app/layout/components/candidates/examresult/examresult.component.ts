import { Component, OnInit } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { FormBuilder } from "@angular/forms";
import { CustomHttpService } from "app/layout/services/custom-http.service";
import { Router, ActivatedRoute } from "@angular/router";
import { FuseConfigService } from "@fuse/services/config.service";
import { ViewEncapsulation } from "@angular/core";
import { fuseAnimations } from "@fuse/animations";
import { fuseConfig } from "app/fuse-config";
import * as shape from 'd3-shape';
import { DataService } from '../candidate.service';
import { TestDataService } from 'app/layout/services/test-data-service.service';
import { CandidateService } from 'app/layout/services/candidate.service';
import { fuseFullScreenConfig } from 'app/layout/entities/globalConstants';

@Component({
    selector: 'cg-examresult',
    templateUrl: './examresult.component.html',
    styleUrls: ['./examresult.component.scss'],
    animations: fuseAnimations,
    standalone: false
})
export class ExamresultComponent implements OnInit {

    public isSpinner: boolean = false;

    color = 'primary';
    mode = 'determinate';
    value = 50;
    testId: any;
    testReport: any = {
        attemptedQuestion: 0,
        skippedQuestion: 0,
        reviewedQuestion: 0,
        correctQuestion: 0,
        inCorrectQuestion: 0,
        maximumMark: 0,
        rightMark: 0,
        negativeMark: 0,
        totalMark: 0,
        userId: "",
        testId: 0
    };

    timeTaken: string;

    attemptedQuestions = [];
    marksPieChart = [];
    timePieChart = [];

    pieChartView: any[] = [300, 100];
    colorScheme = { domain: ['#0099e6', '#e6b800'] };
    colorSchemeMarks = { domain: ['#33cc33', '#ff0000'] };
    colorSchemeTime = { domain: ['#ff0000', '#62d801'] };
    public errorMsg = [];

    fuseConfig = fuseConfig.layout.toolbar.hidden;

    constructor(private _candidateService: CandidateService,
        private router: Router,
        public dataService: DataService,
        private testDataService: TestDataService,
        private _fuseConfigService: FuseConfigService) {
        this._fuseConfigService.config = fuseFullScreenConfig;
        this.fuseConfig = this._fuseConfigService._config.layout.toolbar.hidden;

    }

    ngOnInit() {
        this.testId = this.testDataService.submittedTestId;

        if (this.testId == 0) {
            this.router.navigate(['./layout/candidates/my-test']);
            return;
        }

        this.getExamReport()
    }

    getExamReport() {
        this.showSpinner();
        this._candidateService.GetExamReport(this.testId)
            .subscribe(res => {
                this.hideSpinner();
                this.testReport = res;
                this.timeTaken = this.convertSeconds(this.testReport.timeTaken);
                this.attemptedQuestions = [
                    {
                        "name": "Attempted Questions",
                        "value": this.testReport.attemptedQuestion
                    },
                    {
                        "name": "Skipped Questions",
                        "value": this.testReport.skippedQuestion
                    }
                ];
                this.marksPieChart = [
                    {
                        "name": "Positive Marks",
                        "value": this.testReport.rightMark
                    },
                    {
                        "name": "Negative Marks",
                        "value": this.testReport.negativeMark
                    }
                ];
                this.timePieChart = [
                    {
                        "name": "Time Taken",
                        "value": this.dataService.timeTaken
                    },
                    {
                        "name": "Remaining Time",
                        "value": this.dataService.timeRemaining
                    }
                ];
            },
                err => {
                    this.errorMsg = err.error[""][0];
                    console.log(err.message);
                }
            );
    }


    convertSeconds(s) {
        var min = Math.floor(s / 60);
        var sec = s % 60;
        if (min < 0 && sec < 0) {
            return "Time Over";
        }
        else {
            const seconds = sec < 10 ? '0' + sec.toString() : sec
            const minutes = min < 10 ? '0' + min.toString() : min
            return minutes + ': ' + seconds;
        }
    }

    closeWindow() {
        if (window.opener && window.opener !== window) {
            window.close();
        } else {
            this.router.navigate(['./layout/candidates/my-test']);
        }
    }

    onRightClick() {
        return false;
    }

    showSpinner() { this.isSpinner = true }

    hideSpinner() { this.isSpinner = false }

}
