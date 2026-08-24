import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import { Observable } from 'rxjs';
import { CustomHttpService } from './custom-http.service';
import { map } from 'rxjs/operators';

@Injectable()
export class CandidateService {

  constructor(private http: CustomHttpService) { }

  GetCandidateTestsCount() {
    return this.http._get('Candidate/GetCandidateTestsCount')
      .pipe(map((res: any) => { return res }));
  }

  GetCandidateTestsById(candidateId: string) {
    return this.http._get(`Candidate/GetCandidateTestsById/${candidateId}`)
      .pipe(map((res: any) => { return res }));
  }

  GetCandidateTests() {
    return this.http._get('Candidate/GetCandidateTests')
      .pipe(map((res: any) => { return res }));
  }

  GetUpcomingTests() {
    return this.http._get('Candidate/GetUpcomingTests')
      .pipe(map((res: any) => { return res }));
  }

  GetTestInstruction(id: number) {
    return this.http._get(`Candidate/GetInstruction/${id}`)
      .pipe(map((res: any) => { return res }));
  }

  GetQuestionPaper(body: any) {
    return this.http._post(`Candidate/GetQuestionPaper`, body)
      .pipe(map((res: any) => { return res }));
  }

  StartTest(body: any) {
    return this.http._post('Candidate/StartTest', body)
      .pipe(map((res: any) => { return res }));
  }

  SubmitTest(body: any) {
    return this.http._post('Candidate/SubmitTest', body)
      .pipe(map((res: any) => { return res }));
  }

  GetExamReport(id: number) {
    return this.http._get(`Candidate/GetExamReport/${id}`)
      .pipe(map((res: any) => { return res }));
  }

  GetTestByActivation(id: number) {
    return this.http._get(`Candidate/GetTestByActivation/${id}`)
      .pipe(map((res: any) => { return res }));
  }
}