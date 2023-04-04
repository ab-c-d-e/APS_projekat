import { CdkDragDrop, moveItemInArray } from "@angular/cdk/drag-drop";
import { Component, Input } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import jwt_decode from 'jwt-decode';
import { ReferenceDialog, ReferenceDto } from "src/app/Shared/reference-dialog.component";
import { ScientificPaperService } from "src/app/service";
import { SignalRService } from "src/app/service/signalr.service";

@Component({
    selector: 'reference-tab',
    templateUrl: './reference-tab.component.html',
    styleUrls: ['./tabs.component.scss']
})
export class RefenceTabComponent {
    openButtons: boolean = false;
    references: any = [];
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
        this.references = this.paper.references;
        console.log(this.references);
    }
    get paper(): any {
        return this._paper;
    }

    constructor(signalRService: SignalRService, public dialog: MatDialog, private sciPaperService: ScientificPaperService) {
    }

    drop(event: CdkDragDrop<string[]>) {
        moveItemInArray(this.references, event.previousIndex, event.currentIndex);
        console.log(this.references)
    }

    show() {
        this.openButtons = !this.openButtons;
    }

    linkExistingPaper() {
    }

    reference?: ReferenceDto;
    addReference() {
        const dialogRef = this.dialog.open(ReferenceDialog, {
            data: {
                title: this.reference?.title,
                journal: this.reference?.journal,
                year: this.reference?.year,
                authors: this.reference?.authors
            }
        });

        dialogRef.afterClosed().subscribe(result => {
            console.log('The dialog was closed');
            if (result) {

                const data =
                {
                    id: result.id,
                    paperId: this.paper.id,
                    title: result.title,
                    authors: result.authors,
                    journal: result.journal,
                    year: result.year,
                }
                this.sciPaperService.scientificPaperAddNewReferencePost(data).subscribe(res => {
                    console.log(res);
                    this.references.push(res);
                })
            }
        });
    }
}