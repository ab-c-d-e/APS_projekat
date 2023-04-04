import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
    selector: 'dialog-overview-example-dialog',
    template: `
    <h1 mat-dialog-title>Hi</h1>
    <div mat-dialog-content>
        <p>Which user do you want to add?</p>
        <mat-form-field appearance="fill">
        <mat-label>UserName</mat-label>
        <input matInput [(ngModel)]="data">
        </mat-form-field>
    </div>
    <div mat-dialog-actions>
        <button mat-button (click)="onNoClick()">No Thanks</button>
        <button mat-button [mat-dialog-close]="data" cdkFocusInitial>Ok</button>
    </div>
  `,
})
export class DialogOverviewExampleDialog {
    constructor(
        public dialogRef: MatDialogRef<DialogOverviewExampleDialog>,
        @Inject(MAT_DIALOG_DATA) public data: string,
    ) { }

    onNoClick(): void {
        this.dialogRef.close();
    }
}