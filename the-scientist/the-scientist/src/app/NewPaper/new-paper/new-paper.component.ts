import { Component } from '@angular/core';
import jwt_decode from 'jwt-decode';

@Component({
  selector: 'new-paper',
  templateUrl: './new-paper.component.html',
  styleUrls: ['./new-paper.component.scss']
})
export class NewPaperComponent {
  name?:string;
    constructor()
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
