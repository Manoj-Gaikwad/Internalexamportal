import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient, HttpRequest, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class CustomHttpService {
  private baseUrl = 'api/';

  constructor(private http: HttpClient, private router: Router) {}

  // General GET method
  _get<T>(endpoint: string): Observable<T> {
    return this.http.get<T>(this.baseUrl + endpoint);
  }

  // GET file as blob
//   _getFile(endpoint: string): Observable<Blob> {
//   return this.http.get<Blob>(this.baseUrl + endpoint, { responseType: 'blob' as 'json' });
// }

_getFile(endpoint: string): Observable<Blob> {
  return this.http.get(this.baseUrl + endpoint, { responseType: 'blob' });
}
  // General POST method
  _post<T>(endpoint: string, body: any): Observable<T> {
    return this.http.post<T>(this.baseUrl + endpoint, body);
  }

  // General PUT method
  _put<T>(endpoint: string, body: any): Observable<T> {
    return this.http.put<T>(this.baseUrl + endpoint, body);
  }

  // General DELETE method
  _delete<T>(endpoint: string): Observable<T> {
    return this.http.delete<T>(this.baseUrl + endpoint);
  }

  // General PATCH method
  _patch<T>(endpoint: string, body: any): Observable<T> {
    return this.http.patch<T>(this.baseUrl + endpoint, body);
  }
}
