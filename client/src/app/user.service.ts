import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, catchError, map, of, tap } from 'rxjs';
import { User } from './user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private userUrl = 'https://localhost:5001'; 
  constructor(private http:HttpClient) { }

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };
  
  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(environment.apiUrl + "/get")
      .pipe(
        catchError(this.handleError<User[]>('getUsers', []))
      );
    }

  getFilteredUsers(search:string, sort:number):Observable<User[]>{
    let params = new HttpParams();
    params = params.set('search', search);
    params = params.set('sort', sort.toString());

    return this.http.get<User[]>(environment.apiUrl + "/listFilteredUsers",{params})
      .pipe(
        map((response: any) => {
          return response.result;
        }),
        catchError(this.handleError<User[]>('getFilteredUsers',[]))
      );
  }


  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      console.error(error);

      console.log(`${operation} failed: ${error.message}`);

      return of(result as T);
    };
  }
}
