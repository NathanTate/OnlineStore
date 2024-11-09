import { NgModule } from "@angular/core";
import { SharedModule } from "../../shared/shared.module";
import { AdminComponent } from "./admin.component";
import { ProductAdminComponent } from "./product-management/product-admin.component";
import { ProductManageComponent } from "./product-management/product-manage/product-manage.component";
import { OrdersComponent } from "./orders/orders.component";
import { AdminRoutingModule } from "./admin-routing.module";
import { FormsModule } from "@angular/forms";
import { CategoriesComponent } from './categories/categories.component';
import { SubcategoriesComponent } from './subcategories/subcategories.component';
import { BrandsComponent } from './brands/brands.component';
import { ProductFormComponent } from './product-management/product-form/product-form.component';
import { PhotoFormComponent } from './product-management/photo-form/photo-form.component';


@NgModule({
  declarations: [
    AdminComponent,
    ProductAdminComponent,
    ProductManageComponent,
    OrdersComponent,
    CategoriesComponent,
    SubcategoriesComponent,
    BrandsComponent,
    ProductFormComponent,
    PhotoFormComponent
  ],
  imports: [
    SharedModule,
    AdminRoutingModule,
    FormsModule
  ],
  exports: [
  ]
})
export class AdminModule {

}