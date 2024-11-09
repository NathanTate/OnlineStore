import { NgModule } from "@angular/core";
import { CatalogComponent } from "./catalog.component";
import { SharedModule } from "../../shared/shared.module";
import { FormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { canActivate } from "../../_guards/canActive.guard";
import { FilterComponent } from "../../reusable/filter/filter.component";
import { AdBannerComponent } from "../../reusable/ad-banner/ad-banner.component";

@NgModule({
  declarations: [
    CatalogComponent,
    FilterComponent,
    AdBannerComponent
  ],
  imports: [
    SharedModule,
    FormsModule,
    RouterModule.forChild([
      {path: '', component: CatalogComponent, canActivate: [canActivate]},
    ])
  ]
})
export class CatalogModule {

}