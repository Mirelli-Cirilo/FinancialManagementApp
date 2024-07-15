import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {Transaction} from "../models/transaction";
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {

  private apiUrl = 'https://localhost:5122/api/transaction';
  constructor(private httpClient: HttpClient) { }

  getTransactions() : Observable<Transaction[]> {
    return this.httpClient.get<Transaction[]>(`${this.apiUrl}`);
  }

  getTransactionId(id: number) : Observable<Transaction> {
    return this.httpClient.get<Transaction>(`${this.apiUrl}/${id}`);
  }

  createTransaction(transaction: Transaction) : Observable<Transaction[]>{
    return this.httpClient.post<Transaction[]>(`${this.apiUrl}`, transaction);
  }
  
  editarTransaction(transaction: Transaction) : Observable<Transaction[]> {
    return this.httpClient.put<Transaction[]>(`${this.apiUrl}/${transaction.id}`, transaction);
  }

  excluirTransaction(id: number) : Observable<Transaction[]> {
    return this.httpClient.delete<Transaction[]>(`${this.apiUrl}/${id}`);
  }
}