import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "../../environments/environment.development";
import { OrderCheckoutRequest } from "../_models/Order";
import { map } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  baseUrl = environment.apiUrl + 'order/';
  constructor(private http: HttpClient) {}

  checkout(model: OrderCheckoutRequest) {
    return this.http.post<void>(this.baseUrl + 'checkout', model, {observe: 'response'}).pipe(
      map(response => response.headers.get('Location'))
    );
  }

  validateStripeSession(orderHeaderId: number) {
    return this.http.post<void>(this.baseUrl + 'validateStripeSession/' + orderHeaderId, {});
  }
}