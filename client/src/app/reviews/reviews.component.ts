import { Component, Input, OnInit } from '@angular/core';
import { ReviewService } from './reviews.service';
import { ReviewRequest, ReviewResponse } from '../_models/Review';
import { ToastrService } from 'ngx-toastr';
import { ReviewParams } from '../_models/Params/ReviewParams';
import { Product } from '../_models/Product';
import { ReviewFilterParams } from './reviews-filter';

@Component({
  selector: 'app-reviews',
  templateUrl: './reviews.component.html',
  styleUrl: './reviews.component.css'
})
export class ReviewsComponent implements OnInit{
  @Input({required: true}) product: Product;
  @Input() currentUserId: string;
  reviewResponse: ReviewResponse;

  constructor(private reviewService: ReviewService, private toastr: ToastrService) {
  }

  ngOnInit(): void {
    this.reviewService.params.id = this.product.id;
    this.getReviews();
  }

  onSubmit(review: ReviewRequest) {
    review.productId = this.product.id;
    this.reviewService.addReview(review).subscribe({
      next: () => {
        this.reviewService.invalidateCache();
        this.getReviews();
        this.toastr.success('Thanks for leaving review')
      }
    })
  }

  getReviews() {
    this.reviewService.getReviews().subscribe({
      next: (reviewResponse) => this.reviewResponse = reviewResponse
    })
  }

  deleteReview(id: number) {
    this.reviewService.deleteReview(id).subscribe({
      next: () => {
        this.reviewService.invalidateCache();
        this.getReviews();
        this.toastr.success('Review successfully deleted')
      }
    })
  }

  onFiltersChange(params: ReviewFilterParams) {
    this.reviewService.params = {...this.reviewService.params, ...params}
    this.getReviews();
  }
}
