import { Component, ElementRef, Inject, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatChipInputEvent } from '@angular/material/chips';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable, map, startWith } from 'rxjs';
import { SignalRService } from '../service/signalr.service';
import { ScientificPaperService } from '../service';

export interface ReferenceDto {
  id: number,
  text: string,
  title: string,
  authors: string[],
  journal: string,
  year: number,
  paperId: string
}

@Component({
  selector: 'dialog-overview-example-dialog',
  template: `
  <div mat-dialog-content class="d-flex p-2 flex-column align-items-center"  style="width: 800px; ">
  <p>Reference</p>
      <mat-form-field appearance="fill" style="width: 100%;">
          <mat-label>Title</mat-label>
          <input matInput [(ngModel)]="data.title">
      </mat-form-field>
<br>
      <mat-form-field appearance="fill" style="width: 100%;">
          <mat-label>Journal</mat-label>
          <input matInput [(ngModel)]="data.journal">
      </mat-form-field>
      <br>

      <mat-form-field appearance="fill" style="width: 100%;">
          <mat-label>Year</mat-label>
          <input matInput [(ngModel)]="data.year">
      </mat-form-field>
      <br>

      <mat-form-field appearance="fill" style="width: 100%;">
            <mat-label>Authors</mat-label>
            <mat-chip-grid #chipGrid aria-label="Authors selection">
              <mat-chip-row *ngFor="let author of data.authors" (removed)="remove(author)">
                {{author}}
                <button matChipRemove [attr.aria-label]="'remove ' + author">
                  <mat-icon>cancel</mat-icon>
                </button>
              </mat-chip-row>
            </mat-chip-grid>
            <input placeholder="New Author..." #authorInput [formControl]="authorsCtrl" [matChipInputFor]="chipGrid"
              [matAutocomplete]="auto" [matChipInputSeparatorKeyCodes]="separatorKeysCodes"
              (matChipInputTokenEnd)="add($event)" />
            <mat-autocomplete #auto="matAutocomplete" (optionSelected)="selected($event)">
              <mat-option *ngFor="let author of filteredAuthors | async" [value]="author">
                {{author}}
              </mat-option>
            </mat-autocomplete>
          </mat-form-field>
          <br>

          <mat-form-field appearance="fill" style="width: 100%;">
          <mat-label>Choose existing</mat-label>
          <mat-select (selectionChange)="chagedReference($event.value)"  [(ngModel)]="data.id">
            <mat-option *ngFor="let reference of references" [value]="reference.id">
              {{reference.text}}
            </mat-option>
          </mat-select>
        </mat-form-field>
    </div>
  

  <div mat-dialog-actions>
  <button mat-button (click)="onNoClick()">No Thanks</button>
  <button mat-button [mat-dialog-close]="data" cdkFocusInitial>Ok</button>
</div>
  
  `,
})
export class ReferenceDialog {
  references: any;
  refControl = new FormControl('');
  filteredOptions?: Observable<any[]>;
  constructor(
    private sciPaperService: ScientificPaperService,
    public dialogRef: MatDialogRef<ReferenceDialog>,
    @Inject(MAT_DIALOG_DATA) public data: ReferenceDto
  ) {
    data.authors = [];
    this.filteredAuthors = this.authorsCtrl.valueChanges.pipe(
      startWith(null),
      map((author: string | null) => (author ? this._filter(author) : this.allAuthors.slice())),
    );

    sciPaperService.scientificPaperGetAllReferencesGet().subscribe(
      result => {
        this.references = result;
        this.filteredOptions = this.refControl.valueChanges.pipe(
          startWith(''),
          map(value => {
            const text = typeof value === 'string' ? value : (value as any).text;
            return text ? this._filter(text as string) : this.references.slice();
          }),
        );
      }
    )
  }
  allAuthors: string[] = [];
  onNoClick(): void {
    this.dialogRef.close();
  }
  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();

    return this.allAuthors.filter(author => author.toLowerCase().includes(filterValue));
  }
  items: string[] = [];
  authorsCtrl = new FormControl('');
  separatorKeysCodes = [13, 188]; // Enter and comma
  filteredAuthors: Observable<string[]>;

  addItem(event: any): void {
    const input = event.input;
    const value = event.value;

    // Add item to array
    if ((value || '').trim()) {
      this.items.push(value.trim());
    }

    // Reset the input value
    if (input) {
      input.value = '';
    }

  }

  removeItem(item: string): void {
    const index = this.items.indexOf(item);

    if (index >= 0) {
      this.items.splice(index, 1);
    }
  }

  add(event: MatChipInputEvent): void {
    const value = (event.value || '').trim();

    if (value) {
      this.data.authors.push(value);
    }

    // Clear the input value
    event.chipInput!.clear();

    this.authorsCtrl.setValue(null);
  }

  remove(author: string): void {
    const index = this.data.authors.indexOf(author);

    if (index >= 0) {
      this.data.authors.splice(index, 1);
    }
  }

  @ViewChild('authorInput') authorInput?: ElementRef<HTMLInputElement>;
  selected(event: MatAutocompleteSelectedEvent): void {
    this.data.authors.push(event.option.viewValue);
    if (this.authorInput)
      this.authorInput.nativeElement.value = '';
    this.authorsCtrl.setValue(null);
  }

  selectedRef(event: MatAutocompleteSelectedEvent): void {
    console.log(event.option.viewValue);
  }

  chagedReference(id:number)
  {
    console.log(id);
  }
}