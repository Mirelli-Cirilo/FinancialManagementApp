import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component'; // ajuste o caminho conforme necessário
import { HomeComponent } from './pages/home/home.component';
import { CadastroComponent } from './pages/cadastro/cadastro.component';
import { EditarComponent } from './pages/editar/editar.component';
import { DetailsComponent } from './pages/details/details.component';
import { ExcluirComponent } from './pages/excluir/excluir.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'home', component: HomeComponent},
  { path: 'cadastro', component: CadastroComponent},
  { path: 'editar/:id', component: EditarComponent},
  { path: 'detalhes/:id', component: DetailsComponent},
  { path: 'excluir/:id', component: ExcluirComponent},
  { path: 'dashboard', component: DashboardComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
