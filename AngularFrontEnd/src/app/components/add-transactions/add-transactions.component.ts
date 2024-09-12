import { Component, Input } from '@angular/core';
import { NgForm } from '@angular/forms';
import { TransactionService } from '../../services/transaction.service';
import { AuthService } from '@auth0/auth0-angular';

@Component({
  selector: 'app-add-transactions',
  templateUrl: './add-transactions.component.html',
  styleUrl: './add-transactions.component.css'
})
export class AddTransactionsComponent {
  @Input() budget: any;
  userId: string | undefined;

  constructor(private transactionService: TransactionService, private authService: AuthService) {}

  ngOnInit(): void {
    // Obtenha o UserId do Auth0
    this.authService.user$.subscribe((user) => {
      this.userId = user?.sub; // 'sub' é o campo que contém o UserId no token JWT do Auth0
    });
  }

  onSubmit(form: NgForm) {
    if (form.valid) {
      const transactionData = {
        ...form.value,
        userId: this.userId,
        budgetId: this.budget.id,
      };
  
      console.log('Transaction Data:', transactionData); // Verifique os dados aqui
  
      this.transactionService.createTransaction(transactionData).subscribe(
        response => {
          console.log('Transaction created successfully', response);
          window.location.reload();
        },
        error => {
          console.error('Error creating transaction', error.error.errors);
        }
      );
    }
  }
}
