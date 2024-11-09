import { AfterViewInit, Component, ElementRef, Input, OnInit, QueryList, ViewChild } from '@angular/core';
import { Product } from '../../_models/Product';
import { faCheckCircle, faShoppingCart, faClock } from '@fortawesome/free-solid-svg-icons';
import { ToastrService } from 'ngx-toastr';
import { CartService } from '../../_services/cart.service';

@Component({
  selector: 'app-product-card',
  templateUrl: './product-card.component.html',
  styleUrl: './product-card.component.css'
})
export class ProductCardComponent implements OnInit, AfterViewInit{
   @ViewChild('blur') blurTag: ElementRef<HTMLAnchorElement>;
   @Input() product: Product;
   @Input() index: number;
   productDisplayName: string = '';
   iconCheck = faCheckCircle;
   iconCart = faShoppingCart;
   iconClock = faClock;

   constructor(private toastr: ToastrService, private cartService: CartService) {}

  ngOnInit(): void {
    this.productDisplayName = this.product.name + ':' + this.getProductDisplayName();
  }

  ngAfterViewInit(): void {
    const img = this.blurTag.nativeElement.querySelector('img');

    if(img && img.complete) {
      this.loaded(this.blurTag.nativeElement)
    }
  }

  loaded(el: HTMLElement) {
    el.classList.add('loaded')
  }

  getProductDisplayName() {
    return this.product.productSpecifications
    .reduce((accum: string, spec) => {
      let value = `${accum} ${spec.value}`
      return value;
    }, '')
  }

  addToCart(index: number) {
    this.cartService.addToCart({productId: index, colorId: this.product.colors[0].id, count: 1}).subscribe({
      next: () => {
        this.toastr.success(`${this.product.name} was added to cart`)
      }
    })
  }
}
