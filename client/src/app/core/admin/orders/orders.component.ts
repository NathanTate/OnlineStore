import { Component, OnInit } from '@angular/core';
import { OrderService } from '../../../_services/order.service';
import { OrderParams } from '../../../_models/Params/OrderParams';
import { OrderHeader, OrderResponse } from '../../../_models/Order';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.css'
})
export class OrdersComponent implements OnInit {
  params = new OrderParams();
  orderResponse: OrderResponse;
  constructor (private orderService: OrderService) {}

  ngOnInit(): void {
    this.getOrders();
  }

  getOrders() {
    this.orderService.getOrders(this.params).subscribe({
      next: (response) => {
        this.orderResponse = response;
      }
    });
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
}
