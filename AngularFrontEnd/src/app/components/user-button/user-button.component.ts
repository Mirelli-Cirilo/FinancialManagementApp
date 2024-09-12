import { Component, Input, ElementRef, AfterViewInit  } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { createPopper } from '@popperjs/core';
@Component({
  selector: 'app-user-button',
  templateUrl: './user-button.component.html',
  styleUrl: './user-button.component.css'
})
export class UserButtonComponent implements AfterViewInit {
  @Input() inSidebar: boolean = false;

  constructor(public auth: AuthService, private el: ElementRef) {}

  ngAfterViewInit(): void {
    const button = this.el.nativeElement.querySelector('#userMenu');
    const dropdown = this.el.nativeElement.querySelector('.dropdown-menu');

    const popperInstance = createPopper(button, dropdown, {
      placement: 'bottom-start', // Define a posição inicial
      modifiers: [
        {
          name: 'flip',
          options: {
            fallbackPlacements: ['top-start', 'bottom-start'],
          },
        },
        {
          name: 'preventOverflow',
          options: {
            boundary: 'viewport',
          },
        },
      ],
    });

    button.addEventListener('show.bs.dropdown', () => {
      popperInstance.update();
    });
  }

  logout(): void {
    this.auth.logout({
      logoutParams: {
        returnTo: window.location.origin,
      }
    });
  }

  
}
