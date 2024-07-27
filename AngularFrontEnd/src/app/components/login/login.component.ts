import { Component, OnInit } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import {LoginRequest} from "../../models/login-request";

import { Router} from '@angular/router';
import { NgForm } from '@angular/forms';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {

  invalidLogin?: boolean;
  credentials: LoginRequest = {username:'', password:''};

  constructor(private http: HttpClient, private router: Router, private authService: AuthService) { }
  
  ngOnInit(): void { }

  

  login(form: NgForm) {
    const credentials = {
      'username': form.value.username,
      'password': form.value.password
    }

    this.authService.login(this.credentials).subscribe(
      response => {
        if (response.requiresTwoFactor) {
          // Redirecionar para a página de autenticação de dois fatores
          this.router.navigate(['/twoFactor']);
        } else if (response.token) {
          // Redirecionar para a página inicial ou outra página após o login
          this.router.navigate(['/home']);
        } else {
          // Tratar erro de login
          alert('Login failed');
        }
      },
      error => {
        // Tratar erro de login
        alert('Login failed');
      }
    );
  }
}