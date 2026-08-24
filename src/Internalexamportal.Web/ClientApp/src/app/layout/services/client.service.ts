import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import { Observable } from 'rxjs';
import { CustomHttpService } from './custom-http.service';
import { Paginate } from '../entities/paginate';
import {map} from 'rxjs/operators'

@Injectable()
export class ClientService {

  constructor(private http: CustomHttpService) { }

  GetClientsGrid(body: Paginate) {
    return this.http._post('Client/GetClientsGrid', body)
      .pipe(map((res: any) => { return res }));
  }

  GetClient(id: number) {
    return this.http._get(`Client/Get/${id}`)
      .pipe(map((res: any) => { return res }));
  }

  AddEdit(body: any) {
    return this.http._post('Client/AddEdit', body)
      .pipe(map((res: any) => { return res }));
  }

  // Delete(id: number)
  // {
  //   return this.http._post(`Client/DeleteClient/+${id}`)
  //     .map((res: any) => { return res });
  // }

    DeleteClient(id) {
    console.log("Client Service called")
      return this.http._delete(`client/DeleteClient/${id}`);
  
    }
  

  
  
  GetClients() {
    return this.http._get('Client/GetClients')
      .pipe(map((res: any) => { return res }));
  }
}