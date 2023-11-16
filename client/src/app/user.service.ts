import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, catchError, map, of, tap, throwError } from 'rxjs';
import { User } from './user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http:HttpClient) { }

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };
  
  getUser(uuid:string):Observable<User>{
    return this.http.get<User>(environment.apiUrl + "/User/get/"+uuid)
    .pipe(
      catchError(this.handleError<User>('getUser'))
    );
  }
  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(environment.apiUrl + "/User/get")
      .pipe(
        catchError(this.handleError<User[]>('getUsers', []))
      );
    }

  insertUser(user: any) {
    return this.http.post<any>(environment.apiUrl+ "/User/insert", user, this.httpOptions)
      .pipe(
        map((response: any) => {
          return response;
        }),
        catchError(this.handleError('insertUser', user))
      );
  }

  getFilteredUsers(search:string, sort:number):Observable<User[]>{
    let params = new HttpParams();
    params = params.set('search', search);
    params = params.set('sort', sort.toString());
  
    return this.http.get<User[]>(environment.apiUrl + "/User/listFilteredUsers",{params})
      .pipe(
        map((response: any) => {
          return response.result;
        }),
        catchError(this.handleError<User[]>('getFilteredUsers',[]))
      );
  }
  
  editUserData(uuid:string, data:User):Observable<User>{
    return this.http.put(environment.apiUrl + "/User/update/" + uuid,data)
    .pipe(
      map((response:any)=>{
        return response
      }),
      catchError(this.handleError<any>('editUser',[]))
    );
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
      };
    }
  }
}
