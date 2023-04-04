import { Component, Input } from "@angular/core";
import jwt_decode from 'jwt-decode';

@Component({
    selector: 'sections-tab',
    templateUrl: './sections-tab.component.html',
    styleUrls: ['./tabs.component.scss']
})
export class SectionsTabComponent {
    sections:any;
    creator?: boolean;
    _paper: any;
    @Input()
    set paper(paper: any) {
        this._paper = paper;
        console.log(this._paper)
        const jwt = localStorage.getItem('jwt');
        if (jwt) {
            const payload = jwt_decode(jwt);
            this.creator = (this._paper.creatorId == (payload as any).id);
        }
        this.sections = this.paper.sections;
    }
    get paper(): any {
        return this._paper;
    }
}