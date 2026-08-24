import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { CustomHttpService } from './custom-http.service';
import { SharedService } from './shared.service';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root',
})
export class UserDataService {
  private _user: BehaviorSubject<any> = new BehaviorSubject<any>({});
  public readonly user: Observable<any> = this._user.asObservable();

  private jwtHelper = new JwtHelperService();

  constructor(private http: CustomHttpService, private sharedService: SharedService) {}

  // Update user profile locally
  updateUserProfile(user: any) {
    this._user.next(user);
  }

  // Fetch user details from API if token is valid
  getUserDetails() {
    const token = this.sharedService.getToken();
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      const userId = this.sharedService.decodeTokenToGetUserId();
      this.http._get(`account/GetUserDetails/${userId}`).subscribe({
        next: (res: any) => {
          this._user.next(res);
        },
        error: (err) => {
          console.error('Error fetching user details:', err);
        },
      });
    }
  }
}
