
import { Component, OnInit } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'AngularFrontEnd';
  private hasRedirected = false;  

  constructor(public auth: AuthService, private router: Router) { }

  ngOnInit() {
    this.auth.isAuthenticated$.subscribe(isAuthenticated => {
      if (isAuthenticated && !this.hasRedirected) {
        this.router.navigate(["/dashboard/budgets"]);
        this.hasRedirected = true; 
      }
    });
  }
}
