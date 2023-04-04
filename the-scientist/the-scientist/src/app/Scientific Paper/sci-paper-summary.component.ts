import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import jwt_decode from 'jwt-decode';

@Component({
  selector: 'sci-paper-summary',
  templateUrl: './sci-paper-summary.component.html',
  styleUrls: ['./sci-paper-summary.component.scss']
})
export class ScientificPaperSummaryComponent {
  @Input()
  paper?: any;
  userName?: string;
  constructor(private router: Router) {
    const jwt = localStorage.getItem('jwt');
    if (jwt) {
      const payload = jwt_decode(jwt);
      this.userName = (payload as any).unique_name;
    }
  }

  onClick() {
    this.router.navigate(['/paper', this.paper.id]);
  }
}