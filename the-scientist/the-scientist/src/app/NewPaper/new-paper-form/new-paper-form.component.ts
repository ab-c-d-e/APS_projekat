import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { ScientificPaperService } from 'src/app/service';
import { SignalRService } from 'src/app/service/signalr.service';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { map, startWith } from 'rxjs/operators';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatChipInputEvent } from '@angular/material/chips';

@Component({
  selector: 'new-paper-form',
  templateUrl: './new-paper-form.component.html',
  styleUrls: ['./new-paper-form.component.scss']
})
export class NewPaperFormComponent {
  separatorKeysCodes: number[] = [ENTER, COMMA];
  keywordCtrl = new FormControl('');
  filteredKeywords: Observable<string[]>;

  keywords: string[] = [];
  allKeywords:string[]=[];
  selectedOptions: string[] = [];
  cardValue: any = {
    options: []
  };
  isPublic?: boolean;
  title?: string;
  description?: string;
  journal?: string;
  constructor(signalRService: SignalRService, private sciPaperService: ScientificPaperService, private router: Router) {
    this.sciPaperService.scientificPaperGetAllKeywordsGet().subscribe((result) => {
      this.allKeywords = (result as any[]).map((obj: { name: string }) => obj.name);
    });

    this.filteredKeywords = this.keywordCtrl.valueChanges.pipe(
      startWith(null),
      map((keyword: string | null) => (keyword ? this._filter(keyword) : this.allKeywords.slice())),
    );
  }

  displayFn(option: string): string {
    return option ? option : '';
  }

  selectChange = (event: any) => {
    const key: string = event.key;
    this.cardValue[key] = [...event.data];
    this.selectedOptions = [...event.data];
  };

  add(event: MatChipInputEvent): void {
    const value = (event.value || '').trim();

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

  onSubmit() {
    const data = {
      title: this.title,
      abstract: this.description,
      journal: this.journal,
      keywords: this.keywords,
      isPublic: this.isPublic
    }; 
    this.sciPaperService.scientificPaperNewPost(data).subscribe((result) => {
     this.router.navigate(['/paper', result.id]);
   });
  }

}
