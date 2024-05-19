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
        let img = Object.create(this.product.productImages[0]);
        img.url = 'https://images.unsplash.com/photo-1491472253230-a044054ca35f?q=80&w=2084&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D'
        this.product.productImages.push(img)
        console.log(product)
      }
    })
  }

  onTabChange(tab: string) {
    this.activeTab = tab;
  }

  ngOnDestroy(): void {
    if(this.paramsSubscription) {
      this.paramsSubscription.unsubscribe();
    }
  }
}
