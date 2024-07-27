import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {LoginRequest} from "../models/login-request";
import { Observable, map } from 'rxjs';
import { Register } from '../models/register';
import { Router } from '@angular/router';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'https://localhost:5001/';

  constructor(private httpClient: HttpClient, private router: Router) { }

  login(credentials: LoginRequest): Observable<any> {
    return this.httpClient.post<any>(`${this.apiUrl}api/Auth/login`, credentials)
      .pipe(map(response => {
        localStorage.setItem('UserName', credentials.username); 
        if (response.token) {
          localStorage.setItem('jwt', response.token);
        }
        return response;
      }));
  }

  logout() {
    localStorage.removeItem('jwt');
  }

  isLoggedIn() : boolean {
    return localStorage.getItem('jwt') !== null;
  }

  register(user: Register): Observable<any> {
    return this.httpClient.post(`${this.apiUrl}register`, user);
  }
}  