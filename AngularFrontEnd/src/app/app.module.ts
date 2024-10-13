import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { JwtModule} from '@auth0/angular-jwt';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { HomeComponent } from './pages/home/home.component';

/* Angular Material */
import {MatButtonModule} from '@angular/material/button';
import {MatCardModule} from '@angular/material/card';
import {MatInputModule} from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import {MatTableModule} from '@angular/material/table';
import {MatDialogModule} from '@angular/material/dialog';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { AuthGuard } from './guardh/auth-guard.service';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { HeaderComponent } from './components/header/header.component';
import { HeroComponent } from './components/hero/hero.component';
import { AuthModule } from '@auth0/auth0-angular';
import { AuthHttpInterceptor } from '@auth0/auth0-angular'; 
import { SideNavComponent } from './components/side-nav/side-nav.component';
import { UserButtonComponent } from './components/user-button/user-button.component';
import { DashboardheaderComponent } from './components/dashboardheader/dashboardheader.component';
import { BudgetsComponent } from './components/budgets/budgets.component';
import { CreateBudgetComponent } from './components/create-budget/create-budget.component';
import { TransactionsComponent } from './components/transactions/transactions.component';
import { AddTransactionsComponent } from './components/add-transactions/add-transactions.component';
import { ListTransactionsComponent } from './components/list-transactions/list-transactions.component';
import { EditBudgetComponent } from './components/edit-budget/edit-budget.component';
import { AllDashboardComponent } from './components/all-dashboard/all-dashboard.component';
import { CardInfoComponent } from './components/card-info/card-info.component';
import { LoginComponent} from './components/login/login.component';


export function tokenGetter() {
  return localStorage.getItem('id_token');
}


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    DashboardComponent,
    HeaderComponent,
    HeroComponent,
    SideNavComponent,
    UserButtonComponent,
    DashboardheaderComponent,
    BudgetsComponent,
    CreateBudgetComponent,
    TransactionsComponent,
    AddTransactionsComponent,
    ListTransactionsComponent,
    EditBudgetComponent,
    AllDashboardComponent,
    CardInfoComponent,
    LoginComponent

  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatCardModule,
    MatInputModule,
    MatFormFieldModule,
    MatTableModule,
    MatDialogModule,
    AuthModule.forRoot({
      domain:'https://dev-8zkc8t7vubcg2qoe.us.auth0.com',
      clientId:'XMp1HvRA5Fd65sChBkfKAyuLcoeV4E1g',
      authorizationParams: {
        audience: 'https://financialapi.com',
        redirect_uri: window.location.origin,
        
      },
      httpInterceptor:{
        allowedList:[
          {
            uri: "https://localhost:5001/api/*",
            tokenOptions: {
              authorizationParams: {
              audience: 'https://financialapi.com'
              }
            }
          }
        ]
      }
    }),
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ["localhost:5001"], 
        disallowedRoutes: [""],
      }
    })
  ],
  providers: [AuthGuard, { provide: HTTP_INTERCEPTORS, useClass: AuthHttpInterceptor, multi: true }, provideAnimationsAsync()],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AppModule { }
