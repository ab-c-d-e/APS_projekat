import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatChipsModule } from '@angular/material/chips';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MainNavComponent } from './main-nav/main-nav.component';
import { LayoutModule } from '@angular/cdk/layout';
import { LoginComponent } from './Authentication/login.component';
import { RegisterComponent } from './Authentication/register.component';
import { LogoutComponent } from './Authentication/logout.compnent';
import { HomeComponent } from './Pages/front-page.component';
import { JwtInterceptor } from './service/jwt.interceptor';
import { ScientificPaperComponent } from './Scientific Paper/sci-paper.component';
import { ScientificPaperSummaryComponent } from './Scientific Paper/sci-paper-summary.component';
import { PaperComponent } from './Pages/paper-page.component';
import { HeroComponent } from './Hero/hero.component';
import { RouterModule } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CommonModule } from '@angular/common';
import { NewPaperComponent } from './NewPaper/new-paper/new-paper.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { NewPaperFormComponent } from './NewPaper/new-paper-form/new-paper-form.component';
import { MatSelectModule } from '@angular/material/select';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { NgSelectModule } from '@ng-select/ng-select';
import { SelectAutocompleteComponent } from './Shared/select-autocomplete/select-autocomplete.component';
import { MatTabsModule } from '@angular/material/tabs';
import { EditablePaperComponent } from './Scientific Paper/editable-paper.component';
import { PaperTabComponent } from './Scientific Paper/Tabs/paper-tab.component';
import { SectionsTabComponent } from './Scientific Paper/Tabs/sections-tab.component';
import { UsersTabComponent } from './Scientific Paper/Tabs/users-tab.component';
import { RefenceTabComponent } from './Scientific Paper/Tabs/reference-tab.component';
import { MessagesTabComponent } from './Scientific Paper/Tabs/messages-tab.component';
import { MatDialogModule } from '@angular/material/dialog';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatGridListModule } from '@angular/material/grid-list';
import { DialogOverviewExampleDialog } from './Shared/dialog.component';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { ReferenceDialog } from './Shared/reference-dialog.component';


@NgModule({
  declarations: [
    AppComponent,
    MainNavComponent,
    LoginComponent,
    RegisterComponent,
    LogoutComponent,
    HomeComponent,
    ScientificPaperComponent,
    ScientificPaperSummaryComponent,
    PaperComponent,
    HeroComponent,
    NewPaperComponent,
    NewPaperFormComponent,
    SelectAutocompleteComponent,
    EditablePaperComponent,
    PaperTabComponent,
    SectionsTabComponent,
    UsersTabComponent,
    RefenceTabComponent,
    MessagesTabComponent,
    DialogOverviewExampleDialog,
    ReferenceDialog
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatToolbarModule,
    MatSidenavModule,
    MatIconModule,
    MatListModule,
    MatButtonModule,
    MatCardModule,
    LayoutModule,
    MatInputModule,
    MatProgressSpinnerModule,
    ReactiveFormsModule,
    HttpClientModule,
    MatMenuModule,
    MatProgressBarModule,
    CommonModule,
    FormsModule,
    NgbModule,
    RouterModule,
    BrowserModule,
    NgbModule,
    FormsModule,
    RouterModule,
    AppRoutingModule,
    BrowserModule,
    FormsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatAutocompleteModule,
    NgSelectModule,
    MatCheckboxModule,
    MatChipsModule,
    MatTabsModule,
    MatDialogModule,
    MatPaginatorModule,
    MatGridListModule,
    DragDropModule
  ],
  providers: [{ provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }],
  bootstrap: [AppComponent]
})
export class AppModule { }
