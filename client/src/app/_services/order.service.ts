import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "../../environments/environment.development";
import { OrderCheckoutRequest, OrderResponse } from "../_models/Order";
import { OrderParams } from "../_models/Params/OrderParams";
import { generateHttpParams } from "../shared/httpParamsHelper";
import { map, of } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  baseUrl = environment.apiUrl + 'order/';
  ordersCache = new Map<string, OrderResponse>();
  constructor(private http: HttpClient) {}

  checkout(model: OrderCheckoutRequest) {
    return this.http.post<void>(this.baseUrl + 'checkout', model, {observe: 'response'}).pipe(
      map(response => response.headers.get('Location'))
    );
  }

  validateStripeSession(orderHeaderId: number) {
    return this.http.post<void>(this.baseUrl + 'validateStripeSession/' + orderHeaderId, {});
  }

  getOrders(params: OrderParams) {
    const orders = this.ordersCache.get(Object.values(params).join('-'));

    if(orders) return of(orders);

    let httpParams = generateHttpParams<OrderParams>(params);

    return this.http.get<OrderResponse>(this.baseUrl + 'getOrders', {params: httpParams}).pipe(
      map(orders => {
        this.ordersCache.set(Object.values(params).join('-'), orders);
        return orders;
      })
    )
  }
}