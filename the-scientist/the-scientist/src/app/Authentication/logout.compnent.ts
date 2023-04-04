import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-logout',
  template: `
    <mat-progress-spinner *ngIf="loggingOut" mode="indeterminate"></mat-progress-spinner>
  `,
})
export class LogoutComponent {
  loggingOut = true;
  constructor(private router: Router) {}
  ngOnInit() {
    if(localStorage.getItem('jwt')) localStorage.removeItem('jwt');
    setTimeout(() => {
      this.loggingOut = false;
    }, 2000);
    this.router.navigate(['/']);
  }
}