import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {LoginRequest} from "../models/login-request";
import {LoginResponse} from "../models/login-response";
import { Observable, map } from 'rxjs';
import { Register } from '../models/register';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'https://localhost:5122/';

  constructor(private httpClient: HttpClient) { }

 

 

  enable2Fa(): Observable<any> {
    const token = localStorage.getItem('jwt'); // Certifique-se de ter o token de autenticação
    if (!token) {
      throw new Error('Token de autenticação não encontrado.');
    }
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get(`https://localhost:5122/api/Auth/enable`, { headers  });
  }

  setupTwoFactor(): Observable<any> {
    const token = localStorage.getItem('jwt');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get(`${this.apiUrl}api/Auth/setup`, { headers });
  }

  verifyTwoFactor( code: string): Observable<any> {
    return this.httpClient.post(`${this.apiUrl}api/Auth/verify-2fa`, {code});
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