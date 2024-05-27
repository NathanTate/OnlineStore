import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './core/home/home.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { AuthComponent } from './auth/auth.component';
import { canActivate } from './_guards/canActive.guard';
import { ShoppingCartComponent } from './shopping-cart/shopping-cart.component';
import { CheckoutComponent } from './checkout/checkout.component';
import { AboutComponent } from './static-pages/about/about.component';
import { TermsComponent } from './static-pages/terms/terms.component';
import { ProductComponent } from './core/product/product.component';
import { AdminComponent } from './core/admin/admin.component';
import { ProductAdminComponent } from './core/admin/product-management/product-admin.component';
import { adminGuard } from './_guards/admin.guard';
import { authPageGuard } from './_guards/authPage.guard';

const routes: Routes = [
  {path: '', component: HomeComponent, canActivate: [canActivate]},
  {path: 'admin', component: AdminComponent, canActivate: [adminGuard], children: [
    {path: '', redirectTo: 'product', pathMatch: 'full'},
    {path: 'product', component: ProductAdminComponent}
  ]},
  {path: 'product/:id', component: ProductComponent},
  {path: 'auth', component: AuthComponent, canActivate: [authPageGuard]},
  {path: 'cart', component: ShoppingCartComponent, canActivate: [canActivate]},
  {path: 'checkout', component: CheckoutComponent, canActivate: [canActivate]},
  {path: 'about', component: AboutComponent},
  {path: 'terms', component: TermsComponent},
  {path: '**', component: NotFoundComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {anchorScrolling: 'enabled', scrollPositionRestoration: 'enabled'})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
