import { Component, Input, OnInit } from '@angular/core';
import { Product } from '../../_models/Product';
import { faCheckCircle } from '@fortawesome/free-solid-svg-icons';

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
}
