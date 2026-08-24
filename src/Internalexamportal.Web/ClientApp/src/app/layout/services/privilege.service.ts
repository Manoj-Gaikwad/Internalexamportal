import { Injectable } from '@angular/core';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import { CustomHttpService } from './custom-http.service';
import {map} from 'rxjs/operators';

@Injectable()
export class PrivilegeService {

  constructor(private http: CustomHttpService) { }

  AddRole(body:any){
    return this.http._post(`Role/AddRole`,body)
      .pipe(map((res: any) => { return res }));
  }

  updateRole(body:any){
    return this.http._post(`Role/UpdateRole`,body)
      .pipe(map((res: any) => { return res }));
  }

  updateRoleStatus(roleId:string){
    return this.http._get(`Role/UpdateRoleStatus/${roleId}`)
      .pipe(map((res: any) => { return res }));
  }

  getAllRoles(){
    return this.http._get(`Role`)
      .pipe(map((res: any) => { return res }));
  }

  getRoles(){
    return this.http._get(`Role/GetRoles`)
      .pipe(map((res: any) => { return res }));
  }
  
  getAllPermissions(roleId:string){
    return this.http._get(`Role/GetAllPermissions/${roleId}`)
      .pipe(map((res: any) => { return res }));
  }

  saveRolePermission(body:any){
    return this.http._post(`Role/SaveRolePermission`,body)
      .pipe(map((res: any) => { return res }));
  }
}