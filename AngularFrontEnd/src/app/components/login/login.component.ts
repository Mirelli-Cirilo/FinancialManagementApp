import { Component, OnInit } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import {LoginRequest} from "../../models/login-request";

import { Router} from '@angular/router';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {
  invalidLogin?: boolean;
  credentials: LoginRequest = {username:'', password:''};
  constructor(private http: HttpClient, private router: Router) { }
  
  ngOnInit(): void { }

  login(form: NgForm) {
    const credentials = {
      'username': form.value.username,
      'password': form.value.password
    }

    this.http.post("https://localhost:5122/api/Auth/login", credentials)
    .subscribe(response => {
      const token = (<any>response).token;
      localStorage.setItem('jwt', token); 
      this.invalidLogin = false; 
      this.router.navigate(['/home']);
    }, err => { 
        this.invalidLogin = true;
    })
  }
}