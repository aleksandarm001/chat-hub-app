import { Injectable } from '@angular/core';
import { User, UserResponse } from '../features/models/user';
import { map, Observable, pipe } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  constructor(
    private http: HttpClient
  ) {}

  getUsers(): Observable<User[]> {
    return this.http.get<any>('http://localhost:5162/api/user/get-users').pipe(
      map(response => response.data) 
    );
  }

}
