import { NgModule } from "@angular/core";
import { ModalComponent } from "./modal/modal.component";
import { CommonModule } from "@angular/common";
import { ReadMoreLessComponent } from "./read-more-less/read-more-less.component";
import { CarouselComponent } from "./carousel/carousel.component";
import { FontAwesomeModule } from "@fortawesome/angular-fontawesome";
import { StarComponent } from "./star-component/star.component";
import { DatatableComponent } from './datatable/datatable.component';
import { PaginationComponent } from "../reusable/pagination/pagination.component";
import { ReactiveFormsModule } from "@angular/forms";


@NgModule({
  declarations: [
    ModalComponent,
    ReadMoreLessComponent,
    CarouselComponent,
    StarComponent,
    DatatableComponent,
    PaginationComponent,
    DatatableComponent,
  ],
  imports: [
    CommonModule,
    FontAwesomeModule,
    ReactiveFormsModule,
  ],
  exports: [
    ModalComponent,
    ReadMoreLessComponent,
    CarouselComponent,
    StarComponent,
    PaginationComponent,
    DatatableComponent,
  ]
}) 
export class SharedModule {
}