import { Component, Input, OnInit } from '@angular/core';
import { Product } from '../../_models/Product';
import { faCheckCircle, faShoppingCart } from '@fortawesome/free-solid-svg-icons';
import { ToastrService } from 'ngx-toastr';
import { CartService } from '../../_services/cart.service';

@Component({
  selector: 'app-product-card',
  templateUrl: './product-card.component.html',
  styleUrl: './product-card.component.css'
})
export class ProductCardComponent implements OnInit{
   @Input() product: Product;
   @Input() index: number;
   productDisplayName: string = '';
   faCheck = faCheckCircle;
   faCart = faShoppingCart;

   constructor(private toastr: ToastrService, private cartService: CartService) {}

  ngOnInit(): void {
    this.productDisplayName = this.product.name + ':' + this.getProductDisplayName();
  }

  getProductDisplayName() {
    return this.product.productSpecifications
    .reduce((accum: string, spec) => {
      let value = `${accum} ${spec.value}`
      return value;
    }, '')
  }

  addToCart(index: number) {
    this.cartService.addToCart({productId: index, count: 1}).subscribe({
      next: () => {
        this.toastr.success(`${this.product.name} was added to cart`)
      }
    })
  }
}
