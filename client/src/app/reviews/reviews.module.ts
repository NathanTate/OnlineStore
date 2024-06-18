import { NgModule } from "@angular/core";
import { ReviewsComponent } from "./reviews.component";
import { ContactUsComponent } from "../static-pages/contact-us/contact-us.component";
import { ReviewComponent } from "./review/review.component";
import { ReviewFormComponent } from "./review-form/review-form.componenet";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { FontAwesomeModule } from "@fortawesome/angular-fontawesome";
import { SharedModule } from "../shared/shared.module";
import { ReviewService } from "./reviews.service";
import { ReviewsFilterComponent } from "./reviews-filter/reviews-filter.componenet";



@NgModule({
  declarations: [
    ReviewsComponent,
    ContactUsComponent,
    ReviewComponent,
    ReviewFormComponent,
    ReviewsFilterComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    FontAwesomeModule,
    SharedModule
  ],
  exports: [
    ReviewsComponent,
    ContactUsComponent,
    ReviewComponent,
    ReviewFormComponent,
    ReviewsFilterComponent
  ],
  providers: [ReviewService]
})
export class ReviewsModule {}