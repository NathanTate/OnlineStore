import { Component, OnInit } from '@angular/core';
import { ProductService } from '../_services/product.service';
import { Product } from '../_models/Product';
import { ProductParams } from '../_models/ProductParams';
import { SubCategory } from '../_models/Categories';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit{
  products: Product[] = [];
  productParams: ProductParams;
  subCategories: SubCategory[] = [];
  availableColors: Set<string> = new Set<string>();
  availableBrands: Set<string> = new Set<string>();

  constructor(private productService: ProductService) {

  }
  
  ngOnInit(): void {
    this.getProducts();
    this.getSubCategories();
  }

  getSubCategories() {
    this.productService.getSubCategories(2).subscribe({
      next: (subCategories) => {
        this.subCategories = subCategories;
      }
    })
  }

  getProducts(filterParams?: ProductParams) {
    this.productService.getProducts(filterParams).subscribe({
      next: (productsData) => {
        this.products = productsData;
        this.availableColors = new Set<string>(this.products.map(p => p.color));
        this.availableBrands = new Set<string>(this.products.flatMap(p => p.brand.brandName));
        this.products.forEach(p => {
          p.productImages.push({id: 0, url: 'https://shorturl.at/kmJMN', isMain: true})
        })
      }
    })
  }
}
