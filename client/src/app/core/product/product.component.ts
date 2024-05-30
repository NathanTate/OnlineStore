import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { Subscription } from 'rxjs';
import { ProductService } from '../../_services/product.service';
import { Product } from '../../_models/Product';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrl: './product.component.css'
})
export class ProductComponent implements OnInit, OnDestroy {
  productId!: number;
  product: Product;
  paramsSubscription: Subscription;
  tabs: string[] = ['About Product', 'Specifications', 'Reviews']
  activeTab: string;
  count: number = 1;
  images: string[] = [];


  constructor(private productService: ProductService, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.paramsSubscription = this.route.params.subscribe({
      next: (params: Params) => {
        this.productId = +params['id']
      }
    })
    this.activeTab = this.tabs[0];
    this.getProduct();
  }

  getProduct() {
    this.productService.getProduct(this.productId).subscribe({
      next: (product: Product) => {
        this.product = product;
        this.images = this.product.productImages.map(x => x.url);
      }
    })
  }

  onTabChange(tab: string) {
    this.activeTab = tab;
  }

  onChange(count: number) {
    console.log(count)
  }

  ngOnDestroy(): void {
    if(this.paramsSubscription) {
      this.paramsSubscription.unsubscribe();
    }
  }
}
