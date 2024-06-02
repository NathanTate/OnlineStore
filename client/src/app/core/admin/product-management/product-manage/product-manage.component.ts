import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../../../_services/product.service';
import { ProductResponse } from '../../../../_models/Product';

@Component({
  selector: 'app-product-manage',
  templateUrl: './product-manage.component.html',
  styleUrl: './product-manage.component.css'
})
export class ProductManageComponent implements OnInit {
  productResponse: ProductResponse;
  loading = false;

  constructor(private productService: ProductService) {}

  ngOnInit(): void {
    this.loading = true;
    this.productService.getProducts().subscribe({
      next: (products: ProductResponse) => {
        this.productResponse = products;
        this.loading = false;
      }
    })
  }
}
