import { Component, OnInit } from '@angular/core';
import { Transaction } from '../../models/transaction';
import { TransactionService } from '../../services/transaction.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-editar',
  templateUrl: './editar.component.html',
  styleUrl: './editar.component.css'
})
export class EditarComponent implements OnInit {
  
  btnAcao: string = 'Editar!'
  btnTitulo: string = 'Editar Transação'
  transaction!: Transaction;

  constructor(private transactionService: TransactionService, private route: ActivatedRoute, private router: Router) {
    
  }

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.transactionService.getTransactionId(id).subscribe((data) => {
      this.transaction = data;
        
    });
  }
  
  editTransaction(transaction : Transaction){

      this.transactionService.editarTransaction(transaction).subscribe(data => {
        this.router.navigate(['/home']);
      });

  }
}
