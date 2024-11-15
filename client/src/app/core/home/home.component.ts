import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../_services/product.service';
import { ProductResponse } from '../../_models/Product';
import { Subscription, forkJoin } from 'rxjs';
import { ProductParams } from '../../_models/Params/ProductParams';
import { CategoryWithSubGroups } from '../../_models/Categories';
import { CategoryService } from '../../_services/category.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit{
  productParams: ProductParams;
  newProducts: ProductResponse;
  customBuilds: ProductResponse;
  mSILaptops: ProductResponse;
  desktops: ProductResponse;
  gamingMonitors: ProductResponse;
  categories: CategoryWithSubGroups[];
  loading: boolean = false;
  categoriesSubscription: Subscription;
  images: string[] = ['/assets/images/slide1.webp', '/assets/images/slide2.webp', '/assets/images/slide3.png']

  constructor(private productService: ProductService, private categoryService: CategoryService) {
    this.productParams = new ProductParams();
    this.productParams.inStock = 'true';
    this.productParams.pageSize = 5;
  }

  ngOnInit(): void {
    this.getCategories();
    this.fetchData();
  }

  fetchData() {
    this.loading = true;
    forkJoin({
      newProducts: this.productService.getProducts({...this.productParams, sortColumn: 'date', pageSize: 10, sortBy: 'desc'}),
      customBuilds: this.productService.getProducts({...this.productParams}),
      MSILaptops: this.productService.getProducts({...this.productParams}),
      Desktops: this.productService.getProducts({...this.productParams}),
      GamingMonitors: this.productService.getProducts({...this.productParams}),
    }).subscribe({
      next: ({newProducts, customBuilds, MSILaptops, Desktops, GamingMonitors}) => {
        this.newProducts = newProducts;
        this.customBuilds = customBuilds;
        this.mSILaptops = MSILaptops;
        this.desktops = Desktops;
        this.gamingMonitors = GamingMonitors;
        this.loading = false
      }
    })
  }

  getCategories() {
    this.categoryService.getCategories();
    this.categoriesSubscription = this.categoryService.categories$.subscribe({
      next: (categories) => {
        if(categories !== null) {
          this.categories = categories;
        }
      }
    })
  }
}
