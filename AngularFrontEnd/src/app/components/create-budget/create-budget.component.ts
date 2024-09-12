import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AuthService } from '@auth0/auth0-angular'; // Importe o AuthService da Auth0
import bootstrap, { Modal } from 'bootstrap';
import { BudgetService } from '../../services/budget.service';



@Component({
  selector: 'app-create-budget',
  templateUrl: './create-budget.component.html',
  styleUrls: ['./create-budget.component.css']
})
export class CreateBudgetComponent implements OnInit {
  private modal: Modal | undefined;
  userId: string | undefined;

  constructor(private budgetService: BudgetService, private authService: AuthService) {}

  ngOnInit(): void {
    const modalElement = document.getElementById('exampleModalCenter');
    if (modalElement) {
      this.modal = new Modal(modalElement);
    }

    // Obtenha o UserId do Auth0
    this.authService.user$.subscribe((user) => {
      this.userId = user?.sub; // 'sub' é o campo que contém o UserId no token JWT do Auth0
    });
  }

  openModal() {
    if (this.modal) {
      this.modal.show();
    } else {
      console.error('Modal instance is not initialized.');
    }
  }

  onSubmit(form: NgForm) {
    if (form.valid) {
      // Adicione o UserId ao valor do formulário
      const budgetData = {
        ...form.value,
        userId: this.userId, // Inclua o UserId no objeto que será enviado ao backend
      };

      this.budgetService.createBudget(budgetData).subscribe(
        response => {
          console.log('Budget created successfully', response);
          window.location.reload();
          if (this.modal) {
            this.modal.hide();
          }
          form.reset();
          
        },
        error => {
          console.error('Error creating budget', error.error.errors);
        }
      );
    }
  }
}