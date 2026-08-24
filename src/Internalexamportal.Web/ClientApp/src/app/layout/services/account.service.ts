import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import {map} from 'rxjs/operators';

@Injectable()
export class AccountService {

  constructor(private http: HttpClient) { }

  login(body: any) {
    return this.http.post(`api/account/token`, body)
      .pipe(map((res: any) => { return res }));
  }

  register(body: any) {
    return this.http.post(`api/account/register`, body)
      .pipe(map((res: any) => { return res }));
  }

  checkifUserExists(body: any) {
    return this.http.post(`api/account/checkifUserNameExists`, body)
      .pipe(map((res: any) => { return res }));
  }

  getUserDetails(id: string) {
    return this.http.get(`api/account/GetUserDetails/${id}`)
      .pipe(map((res: any) => { return res }));
  }

  updateProfile(body: any) {
    return this.http.post(`api/account/UpdateProfile`, body)
      .pipe(map((res: any) => { return res; }));
  }

  changePassword(body: any) {
    return this.http.post(`api/account/changepassword`, body)
      .pipe(map((res: any) => { return res }));
  }
}