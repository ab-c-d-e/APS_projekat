import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './Authentication/login.component';
import { LogoutComponent } from './Authentication/logout.compnent';
import { RegisterComponent } from './Authentication/register.component';
import { HomeComponent } from './Pages/front-page.component';
import { PaperComponent } from './Pages/paper-page.component';
import { NewPaperComponent } from './NewPaper/new-paper/new-paper.component';
import { EditablePaperComponent } from './Scientific Paper/editable-paper.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
   { path: 'login', component: LoginComponent },
   { path: 'register', component: RegisterComponent },
   { path: 'logout', component: LogoutComponent },
   { path: 'papers', component: PaperComponent },
   { path: 'papers/:idKeywords', component: PaperComponent },
   { path: 'newpaper', component: NewPaperComponent },
   { path: 'paper/:id', component: EditablePaperComponent }
 ];
 

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
