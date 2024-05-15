import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { AuthComponent } from './auth/auth.component';
import { canActivate } from './_guards/canActive.guard';
import { StarComponent } from './reusable/star-component/star.component';
import { ShoppingCartComponent } from './shopping-cart/shopping-cart.component';
import { CheckoutComponent } from './checkout/checkout.component';
import { AboutComponent } from './static-pages/about/about.component';
import { TermsComponent } from './static-pages/terms/terms.component';

const routes: Routes = [
  {path: '', component: HomeComponent, canActivate: [canActivate]},
  {path: 'auth', component: AuthComponent},
  {path: 'cart', component: ShoppingCartComponent, canActivate: [canActivate]},
  {path: 'checkout', component: CheckoutComponent, canActivate: [canActivate]},
  {path: 'about', component: AboutComponent},
  {path: 'terms', component: TermsComponent},
  {path: '**', component: NotFoundComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
