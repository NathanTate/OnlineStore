import { NgModule } from "@angular/core";
import { ModalComponent } from "./modal/modal.component";
import { CommonModule } from "@angular/common";
import { ReadMoreLessComponent } from "./read-more-less/read-more-less.component";
import { CarouselComponent } from "./carousel/carousel.component";
import { FontAwesomeModule } from "@fortawesome/angular-fontawesome";
import { StarComponent } from "./star-component/star.component";

@NgModule({
  declarations: [
    ModalComponent,
    ReadMoreLessComponent,
    CarouselComponent,
    StarComponent
  ],
  imports: [
    CommonModule,
    FontAwesomeModule
  ],
  exports: [
    ModalComponent,
    ReadMoreLessComponent,
    CarouselComponent,
    StarComponent
  ]
}) 
export class SharedModule {
}