import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-enable2fa',
  templateUrl: './enable2fa.component.html',
  styleUrls: ['./enable2fa.component.css']
})
export class Enable2faComponent implements OnInit {
  code!: string;
  qrCodeUri!: string;
  

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    this.authService.setupTwoFactor().subscribe(response => {
      this.qrCodeUri = response.qrCodeUri;
    }, error => {
      console.error('Failed to setup two-factor authentication', error);
    });
  }

  verifyTwoFactor() {
    this.authService.verifyTwoFactor(this.code).subscribe(() => {
      console.log('Two-factor authentication successful');
      this.router.navigate(['/home']);
    }, error => {
      console.error('Invalid code', error);
    });
  }
}