import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "../../environments/environment.development";
import { CartDetailRequest, CartResponse } from "../_models/Cart";
import { BehaviorSubject, map, tap } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class CartService {
  baseUrl = environment.apiUrl;
  cartSubject = new BehaviorSubject<CartResponse | null>(null);
  cart$ = this.cartSubject.asObservable();
  itemsInCart: number = 0;

  constructor(private http: HttpClient) {}

  cartExists() {
    return this.http.get<boolean>(this.baseUrl + 'cart/cartExists');
  }

  createCart() {
    return this.http.post<CartResponse>(this.baseUrl + 'cart/createCart', {});
  }

  getCart() {
    return this.http.get<CartResponse>(this.baseUrl + 'cart/getCart').pipe(
      map((response: CartResponse) => {
        this.cartSubject.next(response);
        this.itemsInCart = response.cartDetails.reduce((acc, cartDetail) => {
          return acc += cartDetail.count;
        }, 0)
      })
    );
  }

  addToCart(model: CartDetailRequest) {
    return this.http.put<void>(this.baseUrl + 'cart/addToCart', model).pipe(
      map(() => {
        this.getCart().subscribe();
      })
    );
  }

  removeFromCart(id: number, removeAll: boolean = false) {
    return this.http.delete<void>(this.baseUrl + `cart/removeFromCart/${id}/${removeAll}`).pipe(
      map(() => {
        this.getCart().subscribe();
      })
    );
  }
}