import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { faHeart } from '@fortawesome/free-regular-svg-icons';
import { faRemove } from '@fortawesome/free-solid-svg-icons';
import { CartService } from '../../_services/cart.service';
import { CartDetailResponse } from '../../_models/Cart';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-cart-item',
  templateUrl: './cart-item.component.html',
  styleUrl: './cart-item.component.css'
})
export class CartItemComponent implements OnInit, OnDestroy{
  @Input() item: CartDetailResponse;
  removeIcon = faRemove;
  likeIcon = faHeart;
  count: number = 1;
  timeoutId: ReturnType<typeof setTimeout>;

  constructor(private cartService: CartService, private toastr: ToastrService) {

  }

  ngOnInit(): void {
    this.count = this.item.count;
  }

  removeFromCart() {
    this.cartService.removeFromCart(this.item.id).subscribe({
      next: () => {
        this.toastr.success(this.item.product.name + ' removed successfully')
      }
    })
  }

  onChange(event: number) {
    if(this.timeoutId) {
      clearTimeout(this.timeoutId);
    }
    this.timeoutId = setTimeout(() => this.updateQuantity(), 1000)
  }

  updateQuantity() {
    this.cartService.addToCart({productId: this.item.product.id, count: this.count, countUpdate: true}).subscribe({
      next: () => {
        this.toastr.success('Items update to' + this.count)
      }
    })
  }

  ngOnDestroy(): void {
    if(this.timeoutId) {
      clearTimeout(this.timeoutId);
    }
  }
}
