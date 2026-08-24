import { Injectable } from '@angular/core';

@Injectable()
export class RedirectService {
  // store the URL so we can redirect after logging in
  public redirectUrl: string;
}