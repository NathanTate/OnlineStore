import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
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
export class CartItemComponent implements OnInit{
  @Input() item: CartDetailResponse;
  removeIcon = faRemove;
  likeIcon = faHeart;
  count: number = 0;

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
  
  }
}
