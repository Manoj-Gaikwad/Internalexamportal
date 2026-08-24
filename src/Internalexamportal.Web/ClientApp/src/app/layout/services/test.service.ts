import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import { Observable } from 'rxjs';
import { CustomHttpService } from './custom-http.service';
import { Paginate, SubmittedTestPaginate } from '../entities/paginate';
import { map } from 'rxjs/operators';

@Injectable()
export class TestService {

  constructor(private http: CustomHttpService) { }

  GetTestTypes() {
    return this.http._get(`Test/GetTestTypes`).pipe(
      map((res: any) => { return res }));
  }

  GetTestResults(body: SubmittedTestPaginate) {
    return this.http._post(`Test/GetTestResults`, body)
      .pipe(map((res: any) => { return res }));
  }

  GetTestResultById(testResultId: number) {
    return this.http._get(`Test/GetTestResultById/${testResultId}`)
      .pipe(map((res: any) => { return res }));
  }

  saveInstruction(body: any) {
    return this.http._post(`Test/SaveInstruction`, body)
      .pipe(map((res: any) => { return res }));
  }

  GetTestInstructions() {
    return this.http._get(`Test/GetInstruction`)
      .pipe(map((res: any) => { return res }));
  }

  GetTestSettingTypes(testId: number) {
    return this.http._get(`Test/GetTestSettingTypes/${testId}`)
      .pipe(map((res: any) => { return res }));
  }

  CheckIfTestNameExists(body: any) {
    return this.http._post(`Test/CheckIfTestNameExists`, body)
      .pipe(map((res: any) => { return res }));
  }

  GetTestDetails(testId) {
    return this.http._get(`Test/GetTestDetails/${testId}`)
      .pipe(map((res: any) => { return res }));
  }

  UpdateTestDetails(body: any) {
    return this.http._post(`Test/UpdateTestDetails`, body)
      .pipe(map((res: any) => { return res }));
  }

  GetTestById(testId) {
    return this.http._get(`Test/EditTest/${testId}`)
      .pipe(map((res: any) => { return res }));
  }

  AddTest(body: any) {
    return this.http._post(`Test/SaveTest`, body)
      .pipe(map((res: any) => { return res }));
  }

  SendMail(body: any) {
    return this.http._post(`Test/SendMail`, body)
      .pipe(map((res: any) => { return res }));
  }

  UpdateTest(body: any) {
    return this.http._put(`Test/EditTest`, body)
      .pipe(map((res: any) => { return res }));
  }

  AddTestSettings(body: any) {
    return this.http._post(`Test/TestSetting`, body)
      .pipe(map((res: any) => { return res }));
  }

  UpdateTestSettings(body: any) {
    return this.http._put(`Test/UpdateTestSetting`, body)
      .pipe(map((res: any) => { return res }));
  }

  PublishTest(body: any) {
    return this.http._post(`Test/PublishTest`, body)
      .pipe(map((res: any) => { return res }));
  }

  UpdateTestPublish(body: any) {
    return this.http._put(`Test/UpdateTestPublish`, body)
      .pipe(map((res: any) => { return res }));
  }

  GetTestCodeTypes() {
    return this.http._get(`Test/GetTestCodeType`)
      .pipe(map((res: any) => { return res }));
  }

  SetActivationCode(body: any) {
    return this.http._post(`Test/ActivationCode`, body)
      .pipe(map((res: any) => { return res }));
  }

  UpdateActivationCode(body: any) {
    return this.http._put(`Test/UpdateActivationCode`, body)
      .pipe(map((res: any) => { return res }));
  }

  GetAllTests(paginate: Paginate) {
    return this.http._post(`Test/GetTests`, paginate)
      .pipe(map((res: any) => { return res }));
  }

  GetTestSettings(testId: number) {
    return this.http._get(`Test/GetTestSettings/${testId}`)
      .pipe(map((res: any) => { return res }));
  }

  GetTestQuestions(testId: number) {
    return this.http._get(`Test/GetTestQuestion/${testId}`)
      .pipe(map((res: any) => { return res }));
  }

  GetFilteredQuestionsForTest(testId: number, subjectId: number, subjectTopicId: number, questionTypeId: number) {
    return this.http._get(`Test/GetFilteredQuestionsForTest/${testId}/${subjectId}/${subjectTopicId}/${questionTypeId}`)
      .pipe(map((res: any) => { return res }));
  }

  AddTestQuestion(body: any) {
    return this.http._post(`Test/AddTestQuestion`, body)
      .pipe(map((res: any) => { return res }));
  }

  UpdateTestQuestion(body: any) {
    return this.http._post(`Test/UpdateTestQuestion`, body)
      .pipe(map((res: any) => { return res }));
  }

  SendExamResultEmail(body: any) {
    return this.http._post(`Test/SendExamResultEmail`,body)
      .pipe(map((res: any) => { return res }));
  }

  SendExamResultEmailToCandidate(body:any) {
    return this.http._post(`Test/EmailResultToCandidate`,body)
      .pipe(map((res: any) => { return res }));
  }


  PrintCandidateResult(submittedTestId: number): Observable<Blob> {
  return this.http._getFile(`Test/PrintCandidateResult/${submittedTestId}`);
}

  PrintCertificate(submittedTestId: number) {
    return this.http._getFile(`Test/PrintCertificate/${submittedTestId}`);
  }

  PrintTestReport(testId: number) {
    return this.http._getFile(`Test/PrintTestReport/${testId}`)
      .pipe(map((res: any) => {
         return res
         }));
  }

  PrintTestReportExcel(testId: number): Observable<Blob> {
  return this.http._getFile(`Test/PrintTestReportExcel/${testId}`);
}

  GetSubmittedAnswers(submittedTestId: number) {
    return this.http._get(`Test/GetSubmittedAnswers/${submittedTestId}`)
      .pipe(map((res: any) => { return res }));
  }

  SubmitEvaluationMarks(body: any) {
    return this.http._post(`Test/SaveSubjectiveMarks`, body)
      .pipe(map((res: any) => { return res }));
  }

  SaveQuestionEvaluation(body: any) {
    return this.http._post(`Test/SaveQuestionEvaluation`, body)
      .pipe(map((res: any) => { return res }));
  }
}