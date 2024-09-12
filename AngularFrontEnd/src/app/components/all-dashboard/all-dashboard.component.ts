import { AfterViewInit, Component, ElementRef, ViewChild } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { Chart, registerables } from 'chart.js';
import { BudgetService } from '../../services/budget.service';
import { TransactionService } from '../../services/transaction.service';

@Component({
  selector: 'app-all-dashboard',
  templateUrl: './all-dashboard.component.html',
  styleUrls: ['./all-dashboard.component.css']
})
export class AllDashboardComponent implements AfterViewInit {
  budgets: any[] = [];
  transactions: any[] = [];
  totalAmount = 0;
  totalSpend = 0;

  @ViewChild('myChart') private chartRef!: ElementRef;

  constructor(public auth: AuthService, private budgetService: BudgetService, private transactionService: TransactionService) {}

  ngAfterViewInit() {
    this.budgetService.getBudget().subscribe({
      next: (data: any[]) => {
        console.log(data);
        this.budgets = data;
        
        this.calculateTotals();
        this.createChart();
        this.loadTransactions();
      },
      error: (err) => {
        console.error('Erro ao obter orçamentos:', err);
      }
    });
  }

  getBudgetTitle(budgetId: string): string {
    const budget = this.budgets.find(b => b.id === budgetId);
    return budget ? budget.title : 'Unknown Budget';           
  }

  loadTransactions() {
    this.transactionService.getTransactions().subscribe({
      next: (data: any[]) => {
        this.transactions = data;
      },
      error: (err) => {
        console.error('Erro ao obter transações:', err);
      }
    });
  }

  calculateTotals() {
    this.totalAmount = this.budgets.reduce((sum, budget) => sum + budget.initialAmount, 0);
    this.totalSpend = this.budgets.reduce((sum, budget) => sum + budget.amountSpent, 0);
  }

  createChart() {
    // Garanta que o Chart.js está registrado
    Chart.register(...registerables);

    new Chart(this.chartRef.nativeElement, {
      type: 'bar',
      data: {
        labels: ['Total Amount', 'Total Spend', 'Budget Count'],
        datasets: [{
          label: 'Budget Summary',
          data: [this.totalAmount, this.totalSpend, this.budgets.length],
          backgroundColor: [
            'rgba(75, 192, 192, 0.2)', // Color for Total Amount
            'rgba(255, 99, 132, 0.2)', // Color for Total Spend
            'rgba(153, 102, 255, 0.2)' // Color for Budget Count
          ],
          borderColor: [
            'rgba(75, 192, 192, 1)', // Border color for Total Amount
            'rgba(255, 99, 132, 1)', // Border color for Total Spend
            'rgba(153, 102, 255, 1)' // Border color for Budget Count
          ],
          borderWidth: 1
        }]
      },
      options: {
        scales: {
          x: { beginAtZero: true },
          y: { beginAtZero: true }
        }
      }
    });
  }
}