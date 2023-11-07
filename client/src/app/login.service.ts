import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, catchError, map, of, tap, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  private loginUrl='https://localhost:7215/login'

  constructor(private http:HttpClient) { }

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  login(email: string, password: string): Observable<any> {
    const body = { email, password };
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
  
    return this.http.post(this.loginUrl, body, { headers }).pipe(
      map((response:any)=>{
        this.setToken(response.result.token);
      }),
      catchError(this.handleError<any>('login',[]))
    );
  }
  

  setToken(token:string){
    sessionStorage.setItem('authToken',token)
  }

  getToken(): string | null {
    return sessionStorage.getItem('authToken') || null;
  }

  clearToken() {
    sessionStorage.removeItem('authToken');
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      if (error.status === 400) {
        console.error('Bad Request: ', error);
        console.log(`${operation} failed: ${error.message}`);
        return throwError('Bad Request occurred');

      } else {
        console.error(error);
        console.log(`${operation} failed: ${error.message}`);
        return of(result as T);
      }
    }
  }
}
