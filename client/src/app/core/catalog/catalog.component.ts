import { Component, ElementRef, OnInit } from '@angular/core';
import { ProductService } from '../../_services/product.service';
import { ProductResponse } from '../../_models/Product';
import { FilterParams, ProductParams } from '../../_models/Params/ProductParams';
import { SubCategory } from '../../_models/Categories';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-catalog',
  templateUrl: './catalog.component.html',
  styleUrl: './catalog.component.css'
})
export class CatalogComponent implements OnInit{
  productResponse: ProductResponse;
  productParams: ProductParams;
  loading: boolean = true;
  gridMode: string = 'big';
  subCategories: SubCategory[] = [];
  availableColors: Set<string> = new Set<string>();
  availableBrands: Set<string> = new Set<string>();

  constructor(private productService: ProductService) {
    this.productParams = productService.getProductParams();
    console.log(this.productParams)
  }
  
  ngOnInit(): void {
    this.loadProductsAndSubCategories();
  }

  loadProductsAndSubCategories() {
    this.loading = true;

    forkJoin({
      subCategories: this.productService.getSubCategories(2),
      productsData: this.productService.getProducts(this.productParams),
      colors: this.productService.getColors()
    }).subscribe({
      next: ({ subCategories, productsData, colors}) => {
        this.subCategories = subCategories.flatMap(x => x.subcategories);
        console.log(subCategories)
        this.handleProductData(productsData);
        this.availableColors = new Set<string>(colors.flatMap(c => c.value));
        this.loading = false;
      }
    })
  }

  getSubCategories() {
    this.loading = true;
    this.productService.getSubCategories(2).subscribe({
      next: (subCategories) => {
        this.subCategories = subCategories.flatMap(x => x.subcategories);
        this.loading = false;
      }
    })
  }

  getProducts() {
    this.loading = true;
    this.productParams.page;
    this.productParams.pageSize;

    this.productService.getProducts(this.productParams).subscribe({
      next: (productsData) => {
        this.handleProductData(productsData);
        this.loading = false;
      }
    })
  }

  changeGridMode(mode: string) {
    switch (mode) {
      case 'big': 
        this.productParams.pageSize = 20;
        this.productParams.page = 1;
        this.gridMode = 'big';
        break;
      case 'small': 
        this.productParams.pageSize = 8;
        this.gridMode = 'small'
        break;
      default: this.productParams.pageSize = 20;
    }

    this.getProducts();
  }

  handleProductData(productsData: ProductResponse) {
    this.productResponse = productsData;
    this.availableBrands = new Set<string>(this.productResponse.items.flatMap(p => p.brand.brandName));
  }

  onPageChanged(page: number) {
    if(this.productParams.page !== page) {
      this.productParams.page = page;
      this.getProducts();
    }
  }

  setFilters(filterParams: FilterParams) {
    this.productParams = {... this.productParams, ...filterParams};
    this.getProducts();
  }
}
