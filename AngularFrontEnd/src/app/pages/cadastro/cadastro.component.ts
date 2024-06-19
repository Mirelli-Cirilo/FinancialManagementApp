import { Component } from '@angular/core';
import { Transaction } from '../../models/transaction';
import { TransactionService } from '../../services/transaction.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cadastro',
  templateUrl: './cadastro.component.html',
  styleUrl: './cadastro.component.css'
})
export class CadastroComponent {

  btnAcao = "Cadastrar!"
  btnTitulo = "Cadastrar Transação"
  
  constructor(private transactionService: TransactionService, private router: Router) {

    
  }
  
  createTransaction(transaction: Transaction){
    this.transactionService.createTransaction(transaction).subscribe((data) => {
      this.router.navigate(['home'])
    });
  }
}
