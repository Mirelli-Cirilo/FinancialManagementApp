import { Component } from '@angular/core';
import { Register } from '../../models/register';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {

  user: Register = {
    email: '',
    password: ''
  };


  constructor(private authService: AuthService, private router: Router) {}

  onSubmit(): void {
    this.authService.register(this.user).subscribe((data) => {
      console.log('Registration succesful');
      this.router.navigate(['/home']);
    })
  }
}
