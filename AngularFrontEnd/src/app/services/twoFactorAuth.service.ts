import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, map } from 'rxjs';



@Injectable({
  providedIn: 'root'
})
export class TwoFactorAuthService {

  private apiUrl = 'https://localhost:5001/';

  constructor(private httpClient: HttpClient, private router: Router) { }

  enable2Fa(): Observable<any> {
    const token = localStorage.getItem('jwt'); // Certifique-se de ter o token de autenticação
    if (!token) {
      throw new Error('Token de autenticação não encontrado.');
    }
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get(`https://localhost:5001/api/TwoFactor`, { headers  });
  }

  setupTwoFactor(UserName: string): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.httpClient.post<any>(`${this.apiUrl}api/TwoFactor/setup`, { UserName }, { headers });
  }

  verifyTwoFactor(UserName: string, Code: string): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.httpClient.post<any>(`${this.apiUrl}api/TwoFactor/verify-2fa`, { UserName, Code }, { headers })
      .pipe(map(response => {
        if (response.token) {
          localStorage.setItem('jwt', response.token);
          this.router.navigate(['/home']);
        }
        return response;
      }));
  }

}  