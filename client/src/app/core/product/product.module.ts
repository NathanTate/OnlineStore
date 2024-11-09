import { NgModule } from "@angular/core";
import { ProductComponent } from "./product.component";
import { SharedModule } from "../../shared/shared.module";
import { RouterModule } from "@angular/router";
import { canActivate } from "../../_guards/canActive.guard";
import { FeaturesComponent } from "../../static-pages/features/features.component";
import { ReviewsModule } from "../../reviews/reviews.module";
import { FormsModule } from "@angular/forms";

@NgModule({
  declarations: [
    ProductComponent,
    FeaturesComponent
  ],
  imports: [
    SharedModule,
    ReviewsModule,
    FormsModule,
    RouterModule.forChild([
      {path: ':id', redirectTo: ':id/', pathMatch: 'full'},
      {path: ':id/:reviews', component: ProductComponent, canActivate: [canActivate]}
    ])
  ]
})
export class ProductModule {

}