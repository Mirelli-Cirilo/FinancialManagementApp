import { Component, OnInit } from '@angular/core';
import { BudgetService } from '../../services/budget.service';

@Component({
  selector: 'app-card-info',
  templateUrl: './card-info.component.html',
  styleUrl: './card-info.component.css'
})
export class CardInfoComponent implements OnInit {
  budgets: any[] = [];
  totalAmount = 0;
  totalSpend = 0;

  constructor(private budgetService: BudgetService) { }

  ngOnInit(): void {
    this.budgetService.getBudget().subscribe({
      next: (data: any[]) => {
        console.log(data);
        this.budgets = data;

        this.calculateTotal(); // Chama o cálculo aqui, após os budgets serem carregados
        this.calculateTotalSpend();
      },
      error: (err) => {
        console.error('Erro ao obter orçamentos:', err);
      }
    });
  }

  calculateTotal() {
    this.totalAmount = 0;
    this.budgets.forEach(element => {
      this.totalAmount += element.initialAmount;
    });
  }

  calculateTotalSpend() {
    this.totalSpend = 0;
    this.budgets.forEach(element => {
      this.totalSpend += element.amountSpent;
    })
  }
}