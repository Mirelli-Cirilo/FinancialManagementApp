import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
})
export class LoginComponent implements OnInit {

  constructor(public auth: AuthService, private router: Router) {}

  ngOnInit() {
    this.auth.getAccessTokenSilently().subscribe({
      next: (token) => {
        localStorage.setItem('id_token', token);
        console.log('Token:', token);
        this.router.navigate(["/dashboard/budgets"]);
      },
      error: (err) => {
        console.error('Erro ao obter o token:', err);
      }
    });
  }
}
