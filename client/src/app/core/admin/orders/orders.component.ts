import { Component, OnInit } from '@angular/core';
import { OrderService } from '../../../_services/order.service';
import { OrderParams } from '../../../_models/Params/OrderParams';
import { OrderHeader, OrderResponse, OrderStatus } from '../../../_models/Order';
import { ModalService } from '../../../_services/modal.service';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { ToastrService } from 'ngx-toastr';
import { faEye } from '@fortawesome/free-regular-svg-icons';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.css'
})
export class OrdersComponent implements OnInit {
  params = new OrderParams();
  orderResponse: OrderResponse;
  productToEditId: number;
  order: OrderHeader | undefined;
  keys: string[] = [];
  orderStatuses: string[] = [];
  iconTrash = faTrash;
  iconView = faEye;
  progressMap = new Map<number, number>();

  constructor (private orderService: OrderService, private modalService: ModalService, 
    private toastr: ToastrService) {
      this.params.pageSize = 10;
    }

  ngOnInit(): void {
    this.getOrders();
    this.orderStatuses = OrderStatus;
  }

  onSearchSubmit(searchForm: string) {
    this.params.searchTerm = searchForm ?? '';
    this.getOrders();
  }

  sortTable(column: keyof OrderHeader) {
    const futureSortingOrder = this.params.sortBy === 'desc' ? 'asc' : 'desc'
    this.params.sortBy = futureSortingOrder;
    this.params.sortColumn = column.toString();
    this.getOrders();
  }

  filterStatus(status: string) {
    this.params.orderStatus = status;
    this.getOrders();
  }

  getOrders() {
    this.orderService.getOrders(this.params).subscribe({
      next: (response) => {
        this.orderResponse = response;
        for (let item of response.items) {
          this.progressMap.set(item.id, 0);
        }
      }
    });
  }

  deleteOrder(e: number, id: number) {
    this.progressMap.set(id, e / 10);
    const progress = this.progressMap.get(id);
    if (progress !== undefined && progress > 100) {
      this.orderService.deleteOrder(id).subscribe({
        next: () => {
          this.orderService.ordersCache.clear();
          this.getOrders();
          this.toastr.success(`Order №${id} was successfully deleted`)
        }
      })
    }
  }

  viewDetails(productId: number) {
    this.modalService.openModal('details');
    this.productToEditId = productId;
    this.getDetails();
  }

  getDetails() {
    this.order = this.orderResponse.items.find(el => el.id == this.productToEditId);
    this.keys = this.allColumns(this.order);
  }

  onStatusChange(value: string, headerId: number) {
    this.orderService.updateOrderStatus({orderHeaderId: headerId, orderStatus: value}).subscribe({
      next: () => {
        this.orderResponse.items.find(x => x.id == headerId)!.orderStatus = value;
        console.log('success')
      }
    })
  }

  onPageChanged(page: number) {
    if (this.params.page != page) {
      this.params.page = page;
      this.getOrders();
    }
  }

  get columns(): {column: string, sortable: boolean}[] {
    const keys: {column: string, sortable: boolean}[] = [
    { column: 'id', sortable: true },
    { column: 'email', sortable: true },
    { column: 'phone', sortable: true },
    { column: 'orderStatus', sortable: false },
    { column: 'orderDate', sortable: true },
    { column: 'orderTotal', sortable: true }
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
