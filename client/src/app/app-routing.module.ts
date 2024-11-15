import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import { NotFoundComponent } from './not-found/not-found.component';
import { canActivate } from './_guards/canActive.guard';
import { ShoppingCartComponent } from './shopping-cart/shopping-cart.component';
import { CheckoutComponent } from './checkout/checkout.component';
import { AboutComponent } from './static-pages/about/about.component';
import { TermsComponent } from './static-pages/terms/terms.component';
import { HomeComponent } from './core/home/home.component';
import { ContactUsComponent } from './static-pages/contact-us/contact-us.component';
import { UserOrdersComponent } from './user-orders/user-orders.component';
import { ValidateOrderComponent } from './validate-order/validate-order.component';

const routes: Routes = [
  {path: '', component: HomeComponent, canActivate: [canActivate]},
  {path: 'catalog', loadChildren: () => import('./core/catalog/catalog.module')
    .then(m => m.CatalogModule)},
  {path: 'validate-order/:id', component: ValidateOrderComponent, canActivate: [canActivate]},
  {path: 'orders', component: UserOrdersComponent, canActivate: [canActivate]},
  {path: 'admin', loadChildren: () => import('./core/admin/admin.module')
    .then(m => m.AdminModule)},
  {path: 'product', loadChildren: () => import('./core/product/product.module')
    .then(m => m.ProductModule)},
  {path: 'auth', loadChildren: () => import('./auth/auth.module')
    .then(m => m.AuthModule)},
  {path: 'cart', component: ShoppingCartComponent, canActivate: [canActivate]},
  {path: 'checkout', component: CheckoutComponent, canActivate: [canActivate]},
  {path: 'about', component: AboutComponent},
  {path: 'terms', component: TermsComponent},
  {path: 'contact', component: ContactUsComponent, canActivate: [canActivate]},
  {path: '**', component: NotFoundComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {preloadingStrategy: PreloadAllModules})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
