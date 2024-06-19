import { Component, EventEmitter, OnInit, Output, Input } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Transaction } from '../../models/transaction';

@Component({
  selector: 'app-transaction-form',
  templateUrl: './transaction-form.component.html',
  styleUrl: './transaction-form.component.css'
})
export class TransactionFormComponent implements OnInit {
  @Output() onSubmit = new EventEmitter<Transaction>();
  @Input() btnAcao!: string;
  @Input() btnTitulo!: string;
  @Input() dadosTransaction: Transaction | null = null;

  transactionForm!: FormGroup;

  constructor() {}

  ngOnInit(): void {
    this.transactionForm = new FormGroup({
      id: new FormControl(this.dadosTransaction ? this.dadosTransaction.id : 0),
      description: new FormControl(this.dadosTransaction ? this.dadosTransaction.description : '', [Validators.required]),
      amount: new FormControl(this.dadosTransaction ? this.dadosTransaction.amount : '', [Validators.required]),
      date: new FormControl(this.formatDate(this.dadosTransaction ? new Date(this.dadosTransaction.date) : new Date()), [Validators.required])
    });
  }

  formatDate(date: Date): string {
    const pad = (num: number) => (num < 10 ? '0' : '') + num;
    return `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())}`;
  }
  submit(){
    this.onSubmit.emit(this.transactionForm.value)
  }
}
