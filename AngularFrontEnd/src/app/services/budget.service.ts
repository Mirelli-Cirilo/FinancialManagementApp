import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BudgetService {

  private apiUrl = 'http://localhost:5000/api/budget';  // Substitua pela URL da sua API

  constructor(private http: HttpClient) { }

  createBudget(budgetData: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/create`, budgetData);
  }

  getBudget(): Observable<any> {
    const token = localStorage.getItem('id_token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    return this.http.get(this.apiUrl, { headers });
  }

  getBudgetById(id: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/${id}`);
  }

  updateBudget(budgetData: any): Observable<any> {
    // Certifique-se de que o ID é passado corretamente na URL
    return this.http.put(`${this.apiUrl}/${budgetData.id}`, budgetData);  
  }

  deleteBudget(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
