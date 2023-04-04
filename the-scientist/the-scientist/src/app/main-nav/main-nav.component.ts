import { Component, Input } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';

@Component({
  selector: 'main-nav',
  templateUrl: './main-nav.component.html',
  styleUrls: ['./main-nav.component.scss']
})
export class MainNavComponent {
  @Input() name?:string;
  isLogedIn: boolean;
  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches),
      shareReplay()
    );

  constructor(private breakpointObserver: BreakpointObserver) {
    this.isLogedIn = localStorage.getItem('jwt') ? true : false;
  }
  showOptionsPaper = false;

  toggleOptionsPaper() {
    this.showOptionsPaper = !this.showOptionsPaper;
  }

  showOptionsTask= false;

  toggleOptionsTask() {
    this.showOptionsTask = !this.showOptionsTask;
  }
}
