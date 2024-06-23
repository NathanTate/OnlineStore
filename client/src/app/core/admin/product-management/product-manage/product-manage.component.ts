import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../../../_services/product.service';
import { Product, ProductResponse } from '../../../../_models/Product';
import { ProductParams } from '../../../../_models/Params/ProductParams';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { ModalService } from '../../../../_services/modal.service';
import { ToastrService } from 'ngx-toastr';
import { faEdit, faEye } from '@fortawesome/free-regular-svg-icons';

@Component({
  selector: 'app-product-manage',
  templateUrl: './product-manage.component.html',
  styleUrl: './product-manage.component.css'
})
export class ProductManageComponent implements OnInit {
  productResponse: ProductResponse;
  params: ProductParams;
  product: Product | undefined;
  keys: string[] = [];
  iconTrash = faTrash;
  iconEdit = faEdit;
  iconView = faEye;
  progressMap = new Map<number, number>();

  constructor(private productService: ProductService, private modalService: ModalService, private toastr: ToastrService) {}

  ngOnInit(): void {
    this.params = {...this.productService.getProductParams()};
    this.params.pageSize = 10;
    this.params.categoryId = 0;
    this.getProducts();
  }

  onSearchSubmit(searchTerm: string) {
    console.log(searchTerm)
    this.params.searchTerm = searchTerm ?? '';
    this.getProducts();
  }

  onSortTable(column: string) {
    const futureSortingOrder = this.params.sortBy === 'desc' ? 'asc' : 'desc'
    this.params.sortBy = futureSortingOrder;
    this.params.sortColumn = column.toString();
    this.getProducts();
  }

  onFilterStatus(status: string) {
    console.log(status);
  }

  getProducts() {
    this.productService.getProducts(this.params).subscribe({
      next: (products: ProductResponse) => {
        this.productResponse = products;
        for (let item of products.items) {
          this.progressMap.set(item.id, 0);
        }
      }
    })
  }

  deleteProduct(e: number, id: number) {
    this.progressMap.set(id, e / 10);
    const progress = this.progressMap.get(id);
    if (progress !== undefined && progress > 100) {
      this.productService.deleteProduct(id).subscribe({
        next: () => {
          this.getProducts();
          this.toastr.success(`Product №${id} was successfully deleted`)
        }
      })
    }
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

  get columns(): {column:  string, sortable: boolean}[] {
    const keys: {column:  string, sortable: boolean}[] = [
      { column: 'id', sortable: true },
      { column: 'name', sortable: true },
      { column: 'originalPrice', sortable: true },
      { column: 'salePrice', sortable: true },
      { column: 'brand', sortable: false },
      { column: 'colors', sortable: false }
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
