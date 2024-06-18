import { Component, OnInit } from '@angular/core';
import { OrderService } from '../../../_services/order.service';
import { OrderParams } from '../../../_models/Params/OrderParams';
import { OrderHeader, OrderResponse, OrderStatus } from '../../../_models/Order';
import { Config } from 'datatables.net';
import { ModalService } from '../../../_services/modal.service';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.css'
})
export class OrdersComponent implements OnInit {
  params = new OrderParams();
  orderResponse: OrderResponse;
  dtOptions: Config;
  productToEditId: number;
  order: OrderHeader | undefined;
  keys: string[] = [];
  orderStatuses: string[] = [];
  iconTrash = faTrash;

  constructor (private orderService: OrderService, private modalService: ModalService, private toastr: ToastrService) {}

  ngOnInit(): void {
    this.getOrders();
    this.dtOptions = {
      destroy: true
    }
    this.orderStatuses = OrderStatus;
  }

  getOrders() {
    this.orderService.getOrders(this.params).subscribe({
      next: (response) => {
        this.orderResponse = response;
      }
    });
  }

  deleteOrder(id: number) {
    this.orderService.deleteOrder(id).subscribe({
      next: () => {
        this.orderService.ordersCache.clear();
        this.getOrders();
        this.toastr.success(`Order №${id} was successfully deleted`)
      }
    })
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

  get columns(): Array<keyof OrderHeader> {
    const keys: Array<keyof OrderHeader> = [
      'id',
      'email',
      'phone',
      'orderStatus',
      'orderDate',
      'orderTotal'
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
