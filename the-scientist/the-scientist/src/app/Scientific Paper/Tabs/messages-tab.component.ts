import { Component, Input } from "@angular/core";
import jwt_decode from 'jwt-decode';

@Component({
    selector: 'messages-tab',
    templateUrl: './messages-tab.component.html',
    //styleUrls: ['./editable-paper.component.scss']
})
export class MessagesTabComponent {
    messages: any;
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
        this.messages = this.paper.messages;
    }
    get paper(): any {
        return this._paper;
    }
}