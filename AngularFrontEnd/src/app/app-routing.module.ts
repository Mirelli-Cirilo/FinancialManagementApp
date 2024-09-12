import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { BudgetsComponent } from './components/budgets/budgets.component';
import { TransactionsComponent } from './components/transactions/transactions.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { AllDashboardComponent } from './components/all-dashboard/all-dashboard.component';



const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  
  
  { path: 'home', component: HomeComponent},
  { path: 'dashboard', component: DashboardComponent, children: [
    { path: 'budgets', component: BudgetsComponent},
    { path: 'transactions/:id', component: TransactionsComponent},
    { path: 'allDashboard', component: AllDashboardComponent},
  ]},
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
