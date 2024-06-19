import { Component, OnInit } from '@angular/core';
import { TransactionService } from '../../services/transaction.service';
import { Transaction } from '../../models/transaction';
import { MatDialog } from '@angular/material/dialog';
import { ExcluirComponent } from '../excluir/excluir.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {

  transactions: Transaction[] = [];
  generalTransactions: Transaction[] = [];

  columns = ['Descrição', 'Quantia', 'Data', 'Ações', 'Excluir']
  constructor(private transactionService: TransactionService, public dialog: MatDialog) {
    
  }

  ngOnInit(): void {
    this.transactionService.getTransactions().subscribe(data => {
      data.map((item) => {
        item.date = new Date(item.date!).toLocaleDateString('pt-BR')
      })

      this.transactions = data;
      this.generalTransactions = data;
    });
  }

  search(event : Event){
    const target = event.target as HTMLInputElement;
    const value = target.value.toLowerCase();
     
    this.transactions = this.generalTransactions.filter(transaction => {
      return transaction.description.toLowerCase().includes(value);
    })
  }

  OpenDialog(id: number){
    this.dialog.open(ExcluirComponent, {
      width: '450px',
      height: '450px',
      data: { 
        id: id
      }
      
    });
  }
}
