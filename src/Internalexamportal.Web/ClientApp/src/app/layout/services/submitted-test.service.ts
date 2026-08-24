import { Injectable } from '@angular/core';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import { CustomHttpService } from './custom-http.service';
import { map } from 'rxjs/operators';

@Injectable()
export class SubmittedTestService {

    constructor(private http: CustomHttpService) { }

    allowTestResume(SubmittedTestId: number) {
        return this.http._get(`Test/AllowTestResume/${SubmittedTestId}`)
            .pipe(map((res: any) => { return res }));
    }

    saveSubmittedQuestion(body: any) {
        return this.http._post(`Candidate/SaveSubmittedQuestion`, body)
            .pipe(map((res: any) => { return res }));
    }
    

}