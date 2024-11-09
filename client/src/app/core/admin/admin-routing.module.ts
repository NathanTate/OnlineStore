import { RouterModule, Routes } from "@angular/router";
import { AdminComponent } from "./admin.component";
import { ProductAdminComponent } from "./product-management/product-admin.component";
import { ProductManageComponent } from "./product-management/product-manage/product-manage.component";
import { OrdersComponent } from "./orders/orders.component";
import { adminGuard } from "../../_guards/admin.guard";
import { NgModule } from "@angular/core";
import { CategoriesComponent } from "./categories/categories.component";
import { SubcategoriesComponent } from "./subcategories/subcategories.component";
import { BrandsComponent } from "./brands/brands.component";

const routes: Routes = [
  {path: '', component: AdminComponent, canActivate: [adminGuard], children: [
    {path: '', redirectTo: 'product', pathMatch: 'full'},
    {path: 'product', component: ProductAdminComponent},
    {path: 'manage', component: ProductManageComponent},
    {path: 'product/edit/:id', component: ProductAdminComponent},
    {path: 'orders', component: OrdersComponent},
    {path: 'categories', component: CategoriesComponent},
    {path: 'subcategories', component: SubcategoriesComponent},
    {path: 'brands', component: BrandsComponent},
  ]},
]

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class AdminRoutingModule {

}