import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import { Observable } from 'rxjs';
import { CustomHttpService } from './custom-http.service';
import { Paginate } from '../entities/paginate';
import { map } from 'rxjs/operators';

@Injectable()
export class AdminService {

  constructor(private http: CustomHttpService) { }
  
  getDashboardData() {
    return this.http._get(`admin/GetDashboardData`)
      .pipe(map((res: any) => { return res }));
  }
      
  GetAllAdmins(body:Paginate) {
    return this.http._post(`Admin/GetAllAdmins`,body)
      .pipe(map((res: any) => { return res }));
  }

  GetAllCandidates(body:Paginate) {
    return this.http._post(`Candidate/GetAll`,body)
      .pipe(map((res: any) => { return res }));
  }
      
  changeUserStatus(id:string) {
    return this.http._get(`Account/ChangeUserStatus/${id}`)
      .pipe(map((res: any) => { return res }));
  }

  GetCandidateDetailsById(id:string) {
    return this.http._get(`candidate/EditCandidate/${id}`)
      .pipe(map((res: any) => { return res }));
  }
     
  getUser(id:string) {
    return this.http._get(`admin/GetUser/${id}`)
      .pipe(map((res: any) => { return res }));
  }

  UpdateCandidateDetails(body:any) {
    return this.http._post(`candidate/UpdateCandidateData`,body)
      .pipe(map((res: any) => { return res }));
  }
    
  AddUser(body:any) {
    return this.http._post(`Account/AddUser`,body)
      .pipe(map((res: any) => { return res }));
  }
    
  UpdateUser(body:any) {
    return this.http._post(`admin/UpdateUser`,body)
      .pipe(map((res: any) => { return res }));
  }
 
  AddCandidate(body:any) {
    return this.http._post(`Candidate/AddCandidate`,body)
      .pipe(map((res: any) => { return res }));
  }

  getExportCandidateData(body:any) {
    return this.http._post(`Candidate/GetExportCandidateData`,body)
      .pipe(map((res: any) => { return res }));
  }

  ImportCandidates(body:any) {
    return this.http._post(`Account/ImportCandidates`,body)
      .pipe(map((res: any) => { return res }));
  }
  
  AddGroup(body:any) {
    return this.http._post(`Candidate/AddGroup`,body)
      .pipe(map((res: any) => { return res }));
  }
  
  getAllGroups(){
    return this.http._get(`Candidate/GetAllGroups`)
      .pipe(map((res: any) => { return res }));
  }

  DeleteGroup(id){
    return this.http._delete(`Candidate/DeleteGroup/${id}`)
  }

  getRoles(){
    return this.http._get(`role/GetRoles`)
      .pipe(map((res: any) => { return res }));
  }
  
  
}