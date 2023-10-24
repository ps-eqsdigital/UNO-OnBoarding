import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, catchError, of, tap } from 'rxjs';
import { User } from './user';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private userUrl = 'https://localhost:7215'; 
  constructor(private http:HttpClient) { }

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };
  
  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.userUrl + "/get")
      .pipe(
        tap(_ => console.log('fetched users')),
        catchError(this.handleError<User[]>('getUsers', []))
      );
    }

  getFilteredUsers(search:string, sort:number):Observable<User[]>{
    let params = new HttpParams();
    params = params.set('search', search);
    params = params.set('sort', sort.toString());
    return this.http.get<User[]>(this.userUrl + "/listFilteredUsers",{params})
      .pipe(
        tap(_ => console.log('fetched filtered users')),
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
