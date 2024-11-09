import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule} from '@angular/platform-browser/animations'
import { ToastrModule } from 'ngx-toastr';
import { AuthInterceptor } from './_interceptors/auth.interceptor';
import { ErrorInterceptor } from './_interceptors/error.interceptor';
import { ShoppingCartComponent } from './shopping-cart/shopping-cart.component';
import { CartItemComponent } from './shopping-cart/cart-item/cart-item.component';
import { CheckoutComponent } from './checkout/checkout.component';
import { AboutComponent } from './static-pages/about/about.component';
import { TermsComponent } from './static-pages/terms/terms.component';
import { LoadingSpinnerComponent } from './reusable/loading-spinner/loading-spinner.component';
import { LoadingInterceptor } from './_interceptors/loading.interceptor';
import { HomeComponent } from './core/home/home.component';
import { TestimonialComponent } from './testimonial/testimonial.component';
import { SharedModule } from './shared/shared.module';
import { UserOrdersComponent } from './user-orders/user-orders.component';
import { OrderCardComponent } from './user-orders/order-card/order-card.component';
import { ValidateOrderComponent } from './validate-order/validate-order.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    NotFoundComponent,
    ShoppingCartComponent,
    CartItemComponent,
    CheckoutComponent,
    AboutComponent,
    TermsComponent,
    LoadingSpinnerComponent,
    HomeComponent,
    TestimonialComponent,
    UserOrdersComponent,
    OrderCardComponent,
    ValidateOrderComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    SharedModule,
    FormsModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true},
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
