import { Component, OnInit } from '@angular/core';
import { TransactionService } from '../../services/transaction.service';
import { Transaction } from '../../models/transaction';
import { MatDialog } from '@angular/material/dialog';
import { ExcluirComponent } from '../excluir/excluir.component';
import { JwtHelperService } from '@auth0/angular-jwt';
import { TwoFactorAuthService } from '../../services/twoFactorAuth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {

  transactions: Transaction[] = [];
  generalTransactions: Transaction[] = [];

  columns = ['Descrição', 'Quantia', 'Data', 'Ações', 'Excluir']

  requiresTwoFactor: boolean = false;
  constructor(private transactionService: TransactionService, public dialog: MatDialog, private twofactorService: TwoFactorAuthService, private jwtHelper: JwtHelperService) {
    
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

  enable2Fa() {
    console.log('Token:', localStorage.getItem('jwt'));
    this.twofactorService.enable2Fa().subscribe(response => {
      this.requiresTwoFactor = true;
      console.log('Two Factor enabled');
    }, error => {
      console.error('Failed to enable Two Factor Authentication', error);
    });
  }

  isUserAuthenticated = (): boolean => {
    const token = localStorage.getItem("jwt");
    if (token && !this.jwtHelper.isTokenExpired(token)){
      return true;
    }
    return false;
  }
  
  logOut = () => {
    localStorage.removeItem("jwt");
  }
}

