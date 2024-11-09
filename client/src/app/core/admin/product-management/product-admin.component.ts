import { Component, OnDestroy, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { ProductService } from '../../../_services/product.service';
import { Product } from '../../../_models/Product';

@Component({
  selector: 'app-product-admin',
  templateUrl: './product-admin.component.html',
  styleUrl: './product-admin.component.css'
})
export class ProductAdminComponent implements OnInit, OnDestroy{
  isEdit = false;
  productToEditId: number;
  product: Product;
  routeSubscription: Subscription;
  isLoading: boolean = false;

  constructor(private route: ActivatedRoute, private productService: ProductService) {}

  ngOnInit(): void {
    this.checkEditMode();
  }


  checkEditMode() {
    this.isLoading = true;
    this.routeSubscription = this.route.paramMap.subscribe({
      next: (params: ParamMap) => {
        const id = params.get('id')
        if (id) {
          this.productToEditId = +id;
          this.isEdit = true;
          this.getProduct();
        } else {
          this.createPlaceholder();
        }
      },
      error: () => this.isLoading = false
    })
  }

  getProduct() {
    this.productService.getProduct(this.productToEditId).subscribe({
      next: (product: Product) => {
        this.product = product;
        this.isLoading = false;
      }
    })
  }

  createPlaceholder() {
    this.productService.createPlaceholder().subscribe({
      next: (product: Product) => {
        this.product = product;
        this.isLoading = false;
      }
    });
  }

  ngOnDestroy(): void {
    if (this.routeSubscription) {
      this.routeSubscription.unsubscribe();
    }
  }

}
