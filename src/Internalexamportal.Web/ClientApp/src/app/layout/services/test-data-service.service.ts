import { Injectable } from '@angular/core';

@Injectable()
export class TestDataService {
    //Test Details
    testId: number = 0;
    testName: string = "Quantitative Exam";
    testDuration: number = 0;
    testTotalQuestion: number = 0;
    testTypeId: number = 0;
    testTotalMark: number = 0;
    testInstructionId: number = 0;
    difficultLevelId: number = 0;
    testlinkId: string = "";
    setting: Array<any> = [];

    //Submitted Test Details
    submittedTestId: number = 0;
    isPendingTest: boolean = false;
    timeTaken: number = 0;


    isTestSettingApplied(settingId: number): boolean {

        const setting = this.setting.find(prop => prop.id == settingId);

        if (setting) {
            return setting.isChecked;
        } else {
            return false;
        }
    }
}