import { Component } from '@angular/core';
import { AuthService } from '../service/api/auth.service';
import { LoginDto } from '../service/model/loginDto';
import { RegisterDto } from '../service/model/registerDto';

@Component({
  selector: 'app-auth',
  template: `
    <div *ngIf="!isLoggedIn">
      <h2>Login</h2>
      <form (ngSubmit)="onLoginSubmit()">
        <label for="email">Email:</label>
        <input type="email" id="email" [(ngModel)]="loginDto.email" name="email" required>

        <label for="password">Password:</label>
        <input type="password" id="password" [(ngModel)]="loginDto.password" name="password" required>

        <button type="submit">Login</button>
      </form>
    </div>
    <div *ngIf="isLoggedIn">
      <p>You are logged in as {{ email }}</p>
      <button (click)="onLogout()">Logout</button>
    </div>
    <div>
      <h2>Register</h2>
      <form (ngSubmit)="onRegisterSubmit()">
        <label for="email">Email:</label>
        <input type="email" id="email" [(ngModel)]="registerDto.email" name="email" required>

        <label for="password">Password:</label>
        <input type="password" id="password" [(ngModel)]="registerDto.password" name="password" required>

        <button type="submit">Register</button>
      </form>
    </div>
  `,
})
export class AuthComponent {
  loginDto!: LoginDto;
  registerDto!: RegisterDto;
  isLoggedIn = false;
  email?: string;

  constructor(private authService: AuthService) {}

  // ngOnInit() {
  //   this.isLoggedIn = this.authService.isLoggedIn();
  //   if (this.isLoggedIn) {
  //     this.email = this.authService.getEmail();
  //   }
  // }

  // onLoginSubmit() {
    // this.authService.authLoginPost(this.loginDto).subscribe((result) => {
    //   if (result.success) {
    //     this.isLoggedIn = true;
    //     this.email = result.email;
    //   }
    // });
  // }

  // onLogout() {
  //   this.authService.logout();
  //   this.isLoggedIn = false;
  //   this.email = undefined;
  // }

  // onRegisterSubmit() {
  //   this.authService.authRegisterPost(this.registerDto).subscribe((result) => {
  //     if (result.success) {
  //       this.onLoginSubmit();
  //     }
  //   });
  // }
}