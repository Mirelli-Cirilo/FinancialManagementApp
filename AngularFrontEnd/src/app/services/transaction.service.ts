import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {Transaction} from "../models/transaction";
import { Observable } from 'rxjs';
import { AuthService } from '@auth0/auth0-angular';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {

  private apiUrl = 'https://localhost:5001/api/transaction';
  constructor(private httpClient: HttpClient, private auth: AuthService) { }

  getTransactions() : Observable<Transaction[]> {

    this.auth.getAccessTokenSilently().subscribe(token => {
      localStorage.setItem('id_token', token);
    })
    const token = localStorage.getItem('id_token');

    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    return this.httpClient.get<Transaction[]>('https://localhost:5001/api/transaction', { headers });
  }

  getTransactionId(id: number) : Observable<Transaction> {
    const token = localStorage.getItem('id_token');

    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get<Transaction>(`${this.apiUrl}/${id}`, {headers});
  }

  createTransaction(transaction: Transaction) : Observable<Transaction[]>{
    const token = localStorage.getItem('id_token');

    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.post<Transaction[]>(`${this.apiUrl}`, transaction, {headers});
  }
  
  editarTransaction(transaction: Transaction) : Observable<Transaction[]> {
    const token = localStorage.getItem('id_token');

    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    return this.httpClient.put<Transaction[]>(`${this.apiUrl}/${transaction.id}`, transaction, {headers});
  }

  excluirTransaction(id: number) : Observable<Transaction[]> {
    const token = localStorage.getItem('id_token');

    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.delete<Transaction[]>(`${this.apiUrl}/${id}`, {headers});
  }
}