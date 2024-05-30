import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Feedback } from "../_models/Feedback";
import { environment } from "../../environments/environment.development";

@Injectable({
  providedIn: 'root'
})
export class ReviewService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  leaveFeedback(feedback: Feedback) {
    console.log(feedback)
    // return this.http.post<void>(this.baseUrl + 'review/feedback', feedback);
  }
}