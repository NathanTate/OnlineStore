import { Component, OnInit } from '@angular/core';
import { ProductService } from '../_services/product.service';
import { ProductResponse } from '../_models/Product';
import { ProductParams } from '../_models/ProductParams';
import { SubCategory } from '../_models/Categories';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit{
  productResponse: ProductResponse;
  productParams: ProductParams;
  page: number = 1;
  pageSize: number = 20;
  loading: boolean = true;
  subCategories: SubCategory[] = [];
  availableColors: Set<string> = new Set<string>();
  availableBrands: Set<string> = new Set<string>();

  constructor(private productService: ProductService) {
    this.productParams = productService.getProductParams();
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

  getProducts() {
    this.loading = true;
    this.productParams.page = this.page;
    this.productParams.pageSize = this.pageSize;
    this.productService.getProducts(this.productParams).subscribe({
      next: (productsData) => {
        this.productResponse = productsData;
        this.loading = false;
        this.availableColors = new Set<string>(this.productResponse.items.map(p => p.color));
        this.availableBrands = new Set<string>(this.productResponse.items.flatMap(p => p.brand.brandName));
        this.productResponse.items.forEach(p => {
          p.productImages.push({id: 0, url: 'https://shorturl.at/kmJMN', isMain: true})
        })
      }
    })
  }

  pageChanged(page: number) {
    if(this.productParams.page !== page) {
      this.page = page;
      this.productParams.page = page;
      this.getProducts();
    }
  }

  setFilters(productParams: ProductParams) {
    this.productParams = productParams;
    this.getProducts();
  }
}
