import { Component, Input } from "@angular/core";
import { FormControl } from "@angular/forms";
import { MatDialog } from "@angular/material/dialog";
import { Router } from "@angular/router";
import jwt_decode from 'jwt-decode';
import { DialogOverviewExampleDialog } from "src/app/Shared/dialog.component";
import { AddUserDto, ScientificPaperService } from "src/app/service";
import { SignalRService } from "src/app/service/signalr.service";

@Component({
    selector: 'users-tab',
    templateUrl: './users-tab.component.html',
    styleUrls: ['./tabs.component.scss']
})
export class UsersTabComponent {
    _paper: any;
    creator?: boolean;
    editors?: any;
    reviewers?: any;
    @Input()
    set paper(paper: any) {
        this._paper = paper;
        console.log(this._paper)
        const jwt = localStorage.getItem('jwt');
        if (jwt) {
            const payload = jwt_decode(jwt);
            this.creator = (this._paper.creatorId == (payload as any).id);
        }
        this.editors = this.paper.editors;
        this.reviewers = this.paper.reviewers;
        console.log(this.reviewers)
    }
    get paper(): any {
        return this._paper;
    }
    private hubConnection?: signalR.HubConnection;

    constructor(signalRService: SignalRService, public dialog: MatDialog, private sciPaperService: ScientificPaperService) {
        this.hubConnection = signalRService.startConnection();
        this.hubConnection?.on("NewUser", (message) => {
            console.log(message)
            const newUser =
            {
                id: message.user.id,
                name: message.user.name,
                userName: message.user.userName,
                email: message.user.email
            }
            if (message.roleType == 1)
                this.editors.push(newUser);
            if (message.roleType == 0)
                this.reviewers.push(newUser);
        });
    }

    userName?: string;
    newEditor() {
        const dialogRef = this.dialog.open(DialogOverviewExampleDialog, {
            data: this.userName,
        });

        dialogRef.afterClosed().subscribe(result => {
            console.log('The dialog was closed');
            if (result) {
                const user: AddUserDto =
                {
                    paperId: this.paper.id,
                    userName: result,
                    role: 1
                }
                console.log(user)
                this.sciPaperService.scientificPaperAddUsersPost(user).subscribe(result => {
                    const newUser =
                    {
                        id: result.user.id,
                        name: result.user.name,
                        userName: result.user.userName,
                        email: result.user.email
                    }
                    this.editors.push(newUser);
                })
            }
        });
    }

    newReviewer() {
        const dialogRef = this.dialog.open(DialogOverviewExampleDialog, {
            data: this.userName,
        });

        dialogRef.afterClosed().subscribe(result => {
            console.log('The dialog was closed');
            if (result) {
                const user: AddUserDto =
                {
                    paperId: this.paper.id,
                    userName: result,
                    role: 0
                }

                this.sciPaperService.scientificPaperAddUsersPost(user).subscribe(result => {
                    const newUser =
                    {
                        id: result.user.id,
                        name: result.user.name,
                        userName: result.user.userName,
                        email: result.user.email
                    }
                    this.reviewers.push(newUser);
                })
            }
        });
    }

}