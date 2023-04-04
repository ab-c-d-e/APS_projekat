import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginDto } from '../service';
import { AuthService } from '../service/api/auth.service';

@Component({
  selector: 'login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  constructor(private authService: AuthService, private router: Router) {}
  form: FormGroup = new FormGroup({
    email: new FormControl(''),
    password: new FormControl(''),
  });

  submit() {
    if (this.form.valid) {
      const loginDto:LoginDto ={email:this.form.controls['email'].value, password:this.form.controls['password'].value}
      this.authService.authLoginPost(loginDto).subscribe((result) => {
        if (result.result) {
          console.log(result);
          localStorage.setItem("jwt", result.token);
          this.router.navigate(['/']);
        }
      });
    }
  }
  @Input() error: string | null | undefined;

  @Output() submitEM = new EventEmitter();
}
