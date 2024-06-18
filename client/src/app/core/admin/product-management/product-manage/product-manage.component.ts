import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../../../_services/product.service';
import { Product, ProductResponse } from '../../../../_models/Product';
import { ProductParams } from '../../../../_models/Params/ProductParams';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { ModalService } from '../../../../_services/modal.service';
import { ToastrService } from 'ngx-toastr';
import { faEdit } from '@fortawesome/free-regular-svg-icons';

@Component({
  selector: 'app-product-manage',
  templateUrl: './product-manage.component.html',
  styleUrl: './product-manage.component.css'
})
export class ProductManageComponent implements OnInit {
  productResponse: ProductResponse;
  loading = false;
  params: ProductParams;
  product: Product | undefined;
  keys: string[] = [];
  iconTrash = faTrash;
  iconEdit = faEdit;

  constructor(private productService: ProductService, private modalService: ModalService, private toastr: ToastrService) {}

  ngOnInit(): void {
    this.getProducts();
  }

  getProducts() {
    this.productService.getProducts().subscribe({
      next: (products: ProductResponse) => {
        this.productResponse = products;
        this.loading = false;
      }
    })
  }

  deleteProduct(id: number) {
    this.productService.deleteProduct(id).subscribe({
      next: () => {
        this.getProducts();
        this.toastr.success(`Product №${id} was successfully deleted`)
      }
    })
  }

  viewDetails(productId: number) {
    this.modalService.openModal('product');
    this.getDetails(productId);
  }

  getDetails(id: number) {
    this.productService.getProduct(id).subscribe({
      next: (product: Product) => {
        this.product = product;
        this.keys = this.allColumns(this.product);
      }
    })
  }

  onPageChanged(page: number) {
    if (this.params.page != page) {
      this.params.page = page;
      this.getProducts();
    }
  }

  get columns(): Array<keyof Product> {
    const keys: Array<keyof Product> = [
      'id',
      'name',
      'originalPrice',
      'salePrice',
      'brand',
      'colors'
    ];

    return keys;
  }

  allColumns(obj: any, previousPath = ''): string[] {
    let keys: string[] = [];
  
    Object.keys(obj).forEach((key) => {
      const currentPath = previousPath ? `${previousPath}.${key}` : key;
  
      if (obj[key] === null) {
        obj[key] = ''; 
      }
  
      if (typeof obj[key] !== 'object') {
        keys.push(currentPath);
      }
    });
    
    return keys;
  }
}
