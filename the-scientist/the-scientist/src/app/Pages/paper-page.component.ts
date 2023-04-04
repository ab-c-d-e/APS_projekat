import { Component } from '@angular/core';
import jwt_decode from 'jwt-decode';
import { SignalRService } from '../service/signalr.service';
@Component({
    templateUrl: './paper-page.component.html'
})
export class PaperComponent {
    name:string;
    constructor(private signalRService: SignalRService) 
    { 
        const jwt=localStorage.getItem('jwt');
        if(jwt) 
        {
            const payload = jwt_decode(jwt);
            this.name=(payload as any).name;
            
            
        }
        else this.name="UnAuthorized";
    }
}
