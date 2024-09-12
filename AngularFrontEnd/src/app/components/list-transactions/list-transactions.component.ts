import { Component, Input } from '@angular/core';
import { TransactionService } from '../../services/transaction.service';

@Component({
  selector: 'app-list-transactions',
  templateUrl: './list-transactions.component.html',
  styleUrl: './list-transactions.component.css'
})
export class ListTransactionsComponent {
  @Input() transactions: any[] = [];

  constructor(private transactionService: TransactionService) {}

  onClick(transactionId: number) {
    this.transactionService.excluirTransaction(transactionId).subscribe(
      response => {
        console.log('Transaction Deleted successfully', response);
        window.location.reload();
      }
    )
  }
}
