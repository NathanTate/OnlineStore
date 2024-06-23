import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { HeaderComponent } from './header/header.component';
import { CatalogComponent } from './core/catalog/catalog.component';
import { FooterComponent } from './footer/footer.component';
import { AuthComponent } from './auth/auth.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule} from '@angular/platform-browser/animations'
import { ToastrModule } from 'ngx-toastr';
import { AuthInterceptor } from './_interceptors/auth.interceptor';
import { ErrorInterceptor } from './_interceptors/error.interceptor';
import { ProductCardComponent } from './reusable/product-card/product-card.component';
import { FilterComponent } from './reusable/filter/filter.component';
import { DropdownDirective } from './_directives/dropdown.directive';
import { AdBannerComponent } from './reusable/ad-banner/ad-banner.component';
import { ShoppingCartComponent } from './shopping-cart/shopping-cart.component';
import { CartItemComponent } from './shopping-cart/cart-item/cart-item.component';
import { CheckoutComponent } from './checkout/checkout.component';
import { AboutComponent } from './static-pages/about/about.component';
import { TermsComponent } from './static-pages/terms/terms.component';
import { LoadingSpinnerComponent } from './reusable/loading-spinner/loading-spinner.component';
import { LoadingInterceptor } from './_interceptors/loading.interceptor';
import { ProductComponent } from './core/product/product.component';
import { UiTabsComponent } from './reusable/tabs/ui-tabs/ui-tabs.component';
import { ImageSliderComponent } from './reusable/image-slider/image-slider.component';
import { AdminComponent } from './core/admin/admin.component';
import { ProductAdminComponent } from './core/admin/product-management/product-admin.component';
import { HasRoleDirective } from './_directives/hasRole.directive';
import { CurrencyPipe } from '@angular/common';
import { DragAndDropDirective } from './shared/drag-and-drop.directive';
import { HomeComponent } from './core/home/home.component';
import { TestimonialComponent } from './testimonial/testimonial.component';
import { ProductManageComponent } from './core/admin/product-management/product-manage/product-manage.component';
import { OrdersComponent } from './core/admin/orders/orders.component';
import { ForgotPasswordComponent } from './auth/forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './auth/reset-password/reset-password.component';
import { FeaturesComponent } from './static-pages/features/features.component';
import { SharedModule } from './shared/shared.module';
import { ReviewsModule } from './reviews/reviews.module';
import { UserOrdersComponent } from './user-orders/user-orders.component';
import { OrderCardComponent } from './user-orders/order-card/order-card.component';
import { ValidateOrderComponent } from './validate-order/validate-order.component';
import { HoldableDirective } from './shared/holdable.directive';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    CatalogComponent,
    FooterComponent,
    AuthComponent,
    NotFoundComponent,
    ProductCardComponent,
    FilterComponent,
    DropdownDirective,
    AdBannerComponent,
    ShoppingCartComponent,
    CartItemComponent,
    CheckoutComponent,
    AboutComponent,
    TermsComponent,
    LoadingSpinnerComponent,
    ProductComponent,
    UiTabsComponent,
    ImageSliderComponent,
    AdminComponent,
    ProductAdminComponent,
    HasRoleDirective,
    DragAndDropDirective,
    HoldableDirective,
    HomeComponent,
    TestimonialComponent,
    ProductManageComponent,
    OrdersComponent,
    ForgotPasswordComponent,
    ResetPasswordComponent,
    FeaturesComponent,
    UserOrdersComponent,
    OrderCardComponent,
    ValidateOrderComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FontAwesomeModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    ReviewsModule,
    SharedModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true},
    [CurrencyPipe]
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
