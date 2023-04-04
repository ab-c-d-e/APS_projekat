import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import * as signalR from '@microsoft/signalr';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { RegisterDto } from '../service';
import { AuthService } from '../service/api/auth.service';

@Component({
    selector: 'register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
    constructor(private authService: AuthService, private router: Router) { }
    form: FormGroup = new FormGroup({
        email: new FormControl(''),
        password: new FormControl(''),
        name: new FormControl(''),
        lastName: new FormControl(''),
        userName: new FormControl('')
    });

    submit() {
        if (this.form.valid) {
            const registerDto: RegisterDto = {
                email: this.form.controls['email'].value,
                password: this.form.controls['password'].value,
                firstName: this.form.controls['name'].value,
                lastName: this.form.controls['lastName'].value,
                userName: this.form.controls['userName'].value
            }
            this.authService.authRegisterPost(registerDto).subscribe((result) => {
                if (result.result) {
                    console.log(result);
                    localStorage.setItem("jwt", result.token);//signalR

                    const connection = new HubConnectionBuilder()
                        .withUrl('https://localhost:7149/scientistHub', {
                            skipNegotiation: true,
                            transport: signalR.HttpTransportType.WebSockets,
                            accessTokenFactory: () => getToken() // optional: add an access token for authentication
                        })
                        .build();

                    connection.start()
                        .then(() => {
                            console.log("Connection started");
                        })
                        .catch((err) => {
                            console.error(err.toString());
                        });
                        
                    this.router.navigate(['/']);
                }
            });
        }
    }
    @Input() error: string | null | undefined;

    @Output() submitEM = new EventEmitter();
}
function getToken(): string | Promise<string> {
    throw new Error('Function not implemented.');
}

