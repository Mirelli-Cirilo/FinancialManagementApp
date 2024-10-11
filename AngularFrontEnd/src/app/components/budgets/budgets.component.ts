import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { BudgetService } from '../../services/budget.service';

@Component({
  selector: 'app-budgets',
  templateUrl: './budgets.component.html',
  styleUrls: ['./budgets.component.css']  // Correção aqui
})
export class BudgetsComponent implements OnInit {
  budgetExist: boolean = false;
  budgets: any[] = [];


  private apiUrl = 'http://localhost:5000/api/budget/budgetExist';  // URL do endpoint

  constructor(private http: HttpClient, private budgetService: BudgetService) { }

  ngOnInit(): void {
    this.checkBudget();
  }

  checkBudget(): void {
    const token = localStorage.getItem('id_token'); // Obtenha o token do localStorage
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}` // Adicione o header de autorização
    });

    this.http.get<boolean>(this.apiUrl, { headers }).subscribe({
      next: (result: boolean) => {
        console.log(result);
        this.budgetExist = result;
        if (this.budgetExist) {
          this.getBudgets();
        }
      },
      error: (err) => {
        console.error('Erro ao verificar orçamento:', err);
      }
    });
  }

  getBudgets(): void {
    this.budgetService.getBudget().subscribe({
      next: (data: any[]) => {
        this.budgets = data;
      },
      error: (err) => {
        console.error('Erro ao obter orçamentos:', err);
      }
    });
  }

  calculateProgress(amountSpent: number, initialAmount: number): number {
    if (initialAmount === undefined || initialAmount <= 0) return 0;
    return (amountSpent / initialAmount) * 100;
  }
}
