import { NgModule } from "@angular/core";
import { ModalComponent } from "./modal/modal.component";
import { CommonModule, NgOptimizedImage } from "@angular/common";
import { ReadMoreLessComponent } from "./read-more-less/read-more-less.component";
import { CarouselComponent } from "./carousel/carousel.component";
import { FontAwesomeModule } from "@fortawesome/angular-fontawesome";
import { StarComponent } from "./star-component/star.component";
import { DatatableComponent } from './datatable/datatable.component';
import { PaginationComponent } from "../reusable/pagination/pagination.component";
import { ReactiveFormsModule } from "@angular/forms";
import { HasRoleDirective } from "../_directives/hasRole.directive";
import { DragAndDropDirective } from "./drag-and-drop.directive";
import { HoldableDirective } from "./holdable.directive";
import { UiTabsComponent } from "../reusable/tabs/ui-tabs/ui-tabs.component";
import { ImageSliderComponent } from "../reusable/image-slider/image-slider.component";
import { ProductCardComponent } from "../reusable/product-card/product-card.component";
import { RouterModule } from "@angular/router";


@NgModule({
  declarations: [
    ModalComponent,
    ReadMoreLessComponent,
    CarouselComponent,
    StarComponent,
    DatatableComponent,
    PaginationComponent,
    DatatableComponent,
    HasRoleDirective,
    DragAndDropDirective,
    HoldableDirective,
    UiTabsComponent,
    ImageSliderComponent,
    ProductCardComponent
  ],
  imports: [
    CommonModule,
    FontAwesomeModule,
    ReactiveFormsModule,
    RouterModule,
    NgOptimizedImage
  ],
  exports: [
    ModalComponent,
    ReadMoreLessComponent,
    CarouselComponent,
    StarComponent,
    PaginationComponent,
    DatatableComponent,
    UiTabsComponent,
    HasRoleDirective,
    DragAndDropDirective,
    HoldableDirective,
    CommonModule,
    FontAwesomeModule,
    ReactiveFormsModule,
    ImageSliderComponent,
    ProductCardComponent,
    NgOptimizedImage
  ]
}) 
export class SharedModule {
}