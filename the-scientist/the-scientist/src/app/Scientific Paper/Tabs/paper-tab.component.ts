import { COMMA, ENTER } from "@angular/cdk/keycodes";
import { Component, ElementRef, Input, ViewChild } from "@angular/core";
import { FormControl } from "@angular/forms";
import { MatAutocompleteSelectedEvent } from "@angular/material/autocomplete";
import { MatChipInputEvent } from "@angular/material/chips";
import { Router } from "@angular/router";
import jwt_decode from 'jwt-decode';
import { Observable, map, startWith } from "rxjs";
import { ScientificPaperService } from "src/app/service";
import { SignalRService } from "src/app/service/signalr.service";

@Component({
    selector: 'paper-tab',
    templateUrl: './paper-tab.component.html',
    styleUrls: ['./tabs.component.scss']
})
export class PaperTabComponent {
    separatorKeysCodes: number[] = [ENTER, COMMA];
    keywordCtrl = new FormControl('');
    filteredKeywords: Observable<string[]>;

    keywords: string[] = [];
    allKeywords: string[] = [];
    paperKeywords: string[] = [];
    selectedOptions: string[] = [];
    cardValue: any = {
        options: []
    };
    creator?: boolean;
    _paper: any;
    isEditable: boolean = false;
    statuses = [
        {
            value: 0,
            text: 'Active'
        },
        {
            value: 1,
            text: 'Closed'
        },
        {
            value: 2,
            text: 'InReview'
        },
        {
            value: 3,
            text: 'Published'
        }
    ]
    @Input()
    set paper(paper: any) {
        this._paper = paper;
        console.log(paper);
        const jwt = localStorage.getItem('jwt');
        if (jwt) {
            const payload = jwt_decode(jwt);
            this.creator = (this._paper.creatorId == (payload as any).id);
            console.log(this.creator)
        }

        this.paperKeywords = this.paper.keywords.map((obj: { name: string }) => obj.name);
        this.keywords = this.paperKeywords;
    }
    get paper(): any {
        return this._paper;
    }
    private hubConnection?: signalR.HubConnection;
    constructor(private router: Router, signalRService: SignalRService, private sciPaperService: ScientificPaperService) {
        this.sciPaperService.scientificPaperGetAllKeywordsGet().subscribe((result) => {
            this.allKeywords = (result as any[]).map((obj: { name: string }) => obj.name);
        });

        this.filteredKeywords = this.keywordCtrl.valueChanges.pipe(
            startWith(null),
            map((keyword: string | null) => (keyword ? this._filter(keyword) : this.allKeywords.slice())),
          );
        this.hubConnection = signalRService.startConnection();
        this.hubConnection?.on("EditedPaper", (message) => {
            message.new = true;
            this.paper = message;
        });
    }

    onClickKeyword(event: Event, chip: any) {
        this.router.navigate(['/papers', chip.name]);
    }

    onEdit() {
        this.isEditable = !this.isEditable;
    }

    onClose() {
        this.isEditable = !this.isEditable;
    }

    add(event: MatChipInputEvent): void {
        const value = (event.value || '').trim();

        // Add our fruit
        if (value) {
            this.keywords.push(value);
        }

        // Clear the input value
        event.chipInput!.clear();

        this.keywordCtrl.setValue(null);
    }

    remove(keyword: string): void {
        const index = this.keywords.indexOf(keyword);

        if (index >= 0) {
            this.keywords.splice(index, 1);
        }
    }
    @ViewChild('keywordInput') keywordInput?: ElementRef<HTMLInputElement>;
    selected(event: MatAutocompleteSelectedEvent): void {
        this.keywords.push(event.option.viewValue);
        if (this.keywordInput)
            this.keywordInput.nativeElement.value = '';
        this.keywordCtrl.setValue(null);
    }

    private _filter(value: string): string[] {
        const filterValue = value.toLowerCase();

        return this.allKeywords.filter(keyword => keyword.toLowerCase().includes(filterValue));
    }

    selectChange = (event: any) => {
        const key: string = event.key;
        this.cardValue[key] = [...event.data];
        this.selectedOptions = [...event.data];
    };

    onSave() {
        const data = {
            id: this.paper.id,
            title: this.paper.title,
            abstract: this.paper.abstract,
            journal: this.paper.journal,
            keywords: this.keywords,
            isPublic: this.paper.isPublic,
            status: this.paper.status
        };
        console.log(data)
        this.sciPaperService.scientificPaperEditPut(data).subscribe((result) => {
            console.log(result)
        });
    }

}