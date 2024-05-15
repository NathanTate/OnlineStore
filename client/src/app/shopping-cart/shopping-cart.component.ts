import { Component, OnInit } from '@angular/core';
import { CartService } from '../_services/cart.service';
import { CartResponse } from '../_models/Cart';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-shopping-cart',
  templateUrl: './shopping-cart.component.html',
  styleUrl: './shopping-cart.component.css'
})
export class ShoppingCartComponent implements OnInit{
  cart: CartResponse;
  constructor(private cartService: CartService, private toastr: ToastrService) {

  }

  ngOnInit(): void {
    this.getCart();
  }

  getCart() {
    this.cartService.cart$.subscribe({
      next: (cart: CartResponse | null) => {
        if(cart !== null) {
          this.cart = cart
        }
      }
    });
  }

  removeAllItems() {
    this.cartService.removeFromCart(0, true).subscribe({
      next: () => {
        this.toastr.success('Cart cleared successfully')
      }
    });
  }
}
