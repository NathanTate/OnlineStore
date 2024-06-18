import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ReviewRequest, ReviewResponse } from "../_models/Review";
import { environment } from "../../environments/environment.development";
import { Feedback } from "../_models/Feedback";
import { ReviewParams } from "../_models/Params/ReviewParams";
import { generateHttpParams } from "../shared/httpParamsHelper";
import { map, of } from "rxjs";


@Injectable() 
export class ReviewService {
  baseUrl = environment.apiUrl;
  params: ReviewParams;
  reviewsCache = new Map<string, ReviewResponse>();

  constructor(private http: HttpClient) {
    this.params = new ReviewParams();
  }

  getReviews(params: ReviewParams = this.params) {
    const reviews = this.reviewsCache.get(Object.values(params).join('-'));

    if (reviews) return of(reviews);

    let httpParams = generateHttpParams<ReviewParams>(params);

    return this.http.get<ReviewResponse>(this.baseUrl + 'review/getReviews', {params: httpParams}).pipe(
      map(reviews => {
        this.reviewsCache.set(Object.values(params).join('-'), reviews);
        return reviews;
      })
    )
  }

  addReview(review: ReviewRequest) {
    return this.http.post<void>(this.baseUrl + 'review/createReview', review);
  }

  deleteReview(id: number) {
    return this.http.delete<void>(this.baseUrl + 'review/deleteReview/' + id);
  }

  leaveFeedback(feedback: Feedback) {
    return this.http.post<void>(this.baseUrl + 'review/createFeedback', feedback);
  }

  invalidateCache() {
    this.reviewsCache.clear();
  }


}
