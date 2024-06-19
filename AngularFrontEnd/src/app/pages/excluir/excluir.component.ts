import { Component, OnInit, Inject } from '@angular/core';
import { TransactionService } from '../../services/transaction.service';
import { Router } from '@angular/router';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Transaction } from '../../models/transaction';

@Component({
  selector: 'app-excluir',
  templateUrl: './excluir.component.html',
  styleUrl: './excluir.component.css'
})
export class ExcluirComponent implements OnInit{
  inputData: any;
  transaction!: Transaction;
  constructor(private transactionService: TransactionService, private router: Router, @Inject(MAT_DIALOG_DATA) public data: any, private ref:MatDialogRef<ExcluirComponent>) {

  }

  ngOnInit(): void {
    this.inputData = this.data;
    this.transactionService.getTransactionId(this.inputData.id).subscribe((data) => {
      data.date = new Date(data.date!).toLocaleDateString('pt-BR');
      this.transaction = data;
    });
  }

  Excluir(){
    this.transactionService.excluirTransaction(this.inputData.id).subscribe((data) => {
      this.ref.close();
      window.location.reload();
    })
  }

  Voltar(){
    this.ref.close();
  }
}
