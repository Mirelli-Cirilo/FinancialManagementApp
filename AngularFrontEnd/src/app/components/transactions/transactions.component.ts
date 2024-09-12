import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BudgetService } from '../../services/budget.service';
import { TransactionService } from '../../services/transaction.service';
import { Router } from '@angular/router';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-transactions',
  templateUrl: './transactions.component.html',
  styleUrl: './transactions.component.css'
})
export class TransactionsComponent implements OnInit {

  budgetId: string | null = null;
  budget: any;
  transactions: any[] = []; 
  transactionIdToDelete: number | null = null;
  modalRef: NgbModalRef | null = null;
  constructor(private route: ActivatedRoute, private budgetService: BudgetService, private transactionService: TransactionService, private router: Router, private modalService: NgbModal) {}

  ngOnInit(): void {
    this.budgetId = this.route.snapshot.paramMap.get('id');
    
    if (this.budgetId) {
      this.getBudgetDetails(this.budgetId);
      this.getTransactions();
    }
  }

  getTransactions(): void {
    this.transactionService.getTransactions().subscribe({
      next: (data: any[]) => {
        // Filtra as transações pelo budgetId
        this.transactions = data.filter(transaction => transaction.budgetId.toString() === this.budgetId);
      },
      error: (err) => {
        console.error('Erro ao obter transações:', err);
      }
    });
  }

  getBudgetDetails(id: string): void {
    this.budgetService.getBudgetById(id).subscribe(
      (data) => {
        this.budget = data;
        console.log(this.budget);
      },
      (error) => {
        console.error('Erro ao recuperar os detalhes do budget', error);
      }
    );
  }

  calculateProgress(amountSpent: number, initialAmount: number): number {
    if (initialAmount === undefined || initialAmount <= 0) return 0;
    return (amountSpent / initialAmount) * 100;
  }

  openDeleteModal(content: any) {
    this.modalRef = this.modalService.open(content, { ariaLabelledBy: 'deleteModalLabel' });
  }

  confirmDelete(budgetId: number) {
    this.budgetService.deleteBudget(budgetId).subscribe(data => {
      console.log("Budget Deleted!");
      this.router.navigate(["dashboard/budgets"]);
      if (this.modalRef) {
        this.modalRef.close();
      }
    this.transactionIdToDelete = null;
    }
  ) 
}};
