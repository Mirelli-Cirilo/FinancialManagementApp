import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Dashboard } from '../models/dashboard';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {

  private apiUrl = 'http://localhost:5122/api/Dashboard';

  constructor(private httpClient: HttpClient) { }

  getTransactionDashboard(): Observable<any> {
    return this.httpClient.get<Dashboard[]>(this.apiUrl)
  }
}
