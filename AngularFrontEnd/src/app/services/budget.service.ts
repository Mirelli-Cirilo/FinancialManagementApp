import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BudgetService {

  private apiUrl = 'https://localhost:5001/api/Budget';  // Substitua pela URL da sua API

  constructor(private http: HttpClient) { }

  createBudget(budgetData: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/create`, budgetData);
  }

  getBudget(): Observable<any> {
    return this.http.get(this.apiUrl);
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
