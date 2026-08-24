import { Injectable } from '@angular/core';
import { Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";
import { BehaviorSubject, Subject } from "rxjs";
import { Observable } from "rxjs/internal/Observable";

export class ActivationCode{
  public Id:number;
  public activationTypeId:number;
  public activationType:string;
  public TestId:number;
  public accessCode=new Subject<AccessCode>();
    public commonCode=new Subject<commonCode>();
   public test=new Subject<Test>();
}
export class AccessCode{
     public Id:number;
 public AccessTestCode:number;
  public ActivationCodeId:number;
  public Email:string
}
export class Test{}
export class commonCode{
   public Id:number;
 public commonTestCode:number;
  public ActivationCodeId:number;

}
@Injectable({
  providedIn: 'root'
})
export class DataServicesService {

private messageSource  = new BehaviorSubject('');
 currentApprovalStageMessage = this.messageSource .asObservable();
  private data   = new Subject<ActivationCode>();
 currentData=this.messageSource.asObservable();

private  DisableTestSource = new BehaviorSubject<boolean>(true);
    IsDisabled = this.DisableTestSource.asObservable();

  sendData(wrapper: ActivationCode) {
    this.data.next(wrapper);
  }

  clearData() {
    this.data.next(undefined);
  }

  getData(): Observable<ActivationCode> {
    return this.data.asObservable();
  } 

   changeMessage(message: string) {
 this.messageSource .next(message)
}

  changeFlag(message: boolean) {
 this.DisableTestSource .next(message)
 }
}
