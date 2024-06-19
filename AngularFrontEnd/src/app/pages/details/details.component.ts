import { Component, OnInit } from '@angular/core';
import { TransactionService } from '../../services/transaction.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Transaction } from '../../models/transaction';

@Component({
  selector: 'app-details',
  templateUrl: './details.component.html',
  styleUrl: './details.component.css'
})
export class DetailsComponent implements OnInit{

  transaction?: Transaction;
    
  constructor(private transactionService: TransactionService, private route: ActivatedRoute, private router: Router) {
    
  }

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    this.transactionService.getTransactionId(id).subscribe((data) => {
      const dados = data;
      dados.date = new Date(dados.date!).toLocaleDateString('pt-BR');
      
      this.transaction = data;
    })
  }
}
