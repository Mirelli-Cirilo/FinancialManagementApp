import { Component, OnInit } from '@angular/core';
import { TwoFactorAuthService } from '../../services/twoFactorAuth.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-two-factor-auth',
  templateUrl: './two-factor-auth.component.html',
  styleUrl: './two-factor-auth.component.css'
})
export class TwoFactorAuthComponent implements OnInit {

  code!: string;
  qrCodeUri!: string;
  UserName!: string;

  constructor( private twofactorService: TwoFactorAuthService, private router: Router, private route: ActivatedRoute)
  {
    
  }

  ngOnInit(): void {
    this.UserName = localStorage.getItem('UserName')!;
    if (this.UserName) {
      this.setupTwoFactor();
    } else {
      console.error('UserName is not available in localStorage');
    }
  }

  setupTwoFactor() {
    this.twofactorService.setupTwoFactor(this.UserName).subscribe(
      response => {
        this.qrCodeUri = response.qrCodeUri;
      },
      error => {
        alert('Error setting up two-factor authentication');
      }
    );
  }

  verifyTwoFactor() {
    this.twofactorService.verifyTwoFactor(this.UserName, this.code).subscribe(response => {
      console.log('Two-factor authentication successful');
    }, error => {
      console.error('Invalid code');
    });
  }
}
