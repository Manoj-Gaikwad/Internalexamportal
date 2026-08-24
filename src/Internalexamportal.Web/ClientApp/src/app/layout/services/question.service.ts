import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import { Observable } from 'rxjs';
import { CustomHttpService } from './custom-http.service';
import { QuestionPaginate } from '../entities/paginate';
import {map} from 'rxjs/operators';

@Injectable()
export class QuestionService {

  constructor(private http: CustomHttpService) { }
        
  getAllDifficultyLevels() {
    return this.http._get('Question/GetAllDifficult')
      .pipe(map((res: any) => { return res }));
  }
            
  GetAllSubjects() {
    return this.http._get('Question/GetSubject')
      .pipe(map((res: any) => { return res }));
  }
       
  GetTopic(subjectId:number) {
    return this.http._get(`Question/GetTopic/${subjectId}`)
      .pipe(map((res: any) => { return res }));
  }
            
  GetAllSubjectsWithTopics() {
    return this.http._get('Question/GetAllSubjectsWithTopics')
      .pipe(map((res: any) => { return res }));
  }
           
  GetQuestionTypes() {
    return this.http._get('Question/GetQuestionType')
      .pipe(map((res: any) => { return res }));
  }
         
  GetAllQuestions() {
    return this.http._get('Question/GetQuestion')
      .pipe(map((res: any) => { return res }));
  }
        
  GetQuestionDetails(id:number) {
    return this.http._get(`Question/EditQuestion/${id}`)
      .pipe(map((res: any) => { return res }));
  }
        
  GetQuestionsByFilter(body:QuestionPaginate) {
    return this.http._post(`Question/GetQuestions`,body)
      .pipe(map((res: any) => { return res }));
  }

  AddQuestion(body:any) {
    return this.http._post(`Question/SaveQuestion`,body)
      .pipe(map((res: any) => { return res }));
  }

  UpdateQuestion(body:any){
    return this.http._put(`Question/EditQuestion`,body)
    .pipe(map((res: any) => { return res }));
  }

  AddSubject(body:any) {
    return this.http._post(`Question/AddSubject`,body)
      .pipe(map((res: any) => { return res }));
  }

  GetSubjectDetails(id:number) {
    return this.http._get(`Question/GetSubjectDetails/${id}`)
      .pipe(map((res: any) => { return res }));
  }
  
  EditSubject(body:any) {
    return this.http._put(`Question/EditSubject`,body)
      .pipe(map((res: any) => { return res }));
  }
  
  AddTopic(body:any) {
    return this.http._post(`Question/AddTopic`,body)
      .pipe(map((res: any) => { return res }));
  }

  EditTopic(body:any) {
    return this.http._post(`Question/EditTopic`,body)
      .pipe(map((res: any) => { return res }));
  }
}