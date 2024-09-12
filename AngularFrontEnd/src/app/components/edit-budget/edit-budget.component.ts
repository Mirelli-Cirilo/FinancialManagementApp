import { Component, Input } from '@angular/core';
import { NgForm } from '@angular/forms';
import { BudgetService } from '../../services/budget.service';
import { Modal } from 'bootstrap';

@Component({
  selector: 'app-edit-budget',
  templateUrl: './edit-budget.component.html',
  styleUrls: ['./edit-budget.component.css']
})
export class EditBudgetComponent {
  private modal: Modal | undefined;

  // Recebe o ID do orçamento como entrada (caso esteja editando um orçamento)
  @Input() budget: any;

  constructor(private budgetService: BudgetService) {}

  ngOnInit(): void {
    const modalElement = document.getElementById('exampleModalCenter');
    if (modalElement) {
      this.modal = new Modal(modalElement);
    }
  }

  openModal() {
    if (this.modal) {
      this.modal.show();
    } else {
      console.error('Modal instance is not initialized.');
    }
  }

  onSubmit(form: NgForm) {
    if (form.valid && this.budget) {
      const budgetData = {
        ...form.value,
        id: this.budget.id,  // Certifique-se de que o ID do orçamento está sendo incluído
        userId: this.budget.userId,
        createdAt: new Date()
      };
  
      this.budgetService.updateBudget(budgetData).subscribe(
        response => {
          console.log('Budget updated successfully', response);
          window.location.reload();
          if (this.modal) {
            this.modal.hide();
          }
          form.reset();
        },
        error => {
          console.error('Error updating budget', error.error.errors);
        }
      );
    } else {
      console.log('Form is invalid or BudgetId is missing.');
    }
  }
}