import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { AuthComponent } from './auth/auth.component';
import { canActivate } from './_guards/canActive.guard';
import { StarComponent } from './reusable/star-component/star.component';

const routes: Routes = [
  {path: '', component: HomeComponent, canActivate: [canActivate]},
  {path: 'auth', component: AuthComponent},
  {path: 'star', component: StarComponent},
  {path: '**', component: NotFoundComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
