import { Component, OnInit } from '@angular/core';
import { OrderService } from '../_services/order.service';
import { OrderResponse } from '../_models/Order';
import { OrderParams } from '../_models/Params/OrderParams';

@Component({
  selector: 'app-user-orders',
  templateUrl: './user-orders.component.html',
  styleUrl: './user-orders.component.css'
})
export class UserOrdersComponent implements OnInit {
  ordersResponse: OrderResponse;
  orderParams: OrderParams;

  constructor(private orderService: OrderService) {
    this.orderParams = new OrderParams()
  }

  ngOnInit(): void {
    this.getOrders();
  }

  getOrders() {
    this.orderService.getOrders(this.orderParams).subscribe({
      next: (ordersResponse) => this.ordersResponse = ordersResponse 
    })
  }

  cancelOrder(orderId: number) {
    this.orderService.updateOrderStatus({orderHeaderId: orderId, orderStatus: 'CANCELED'}).subscribe({
      next: () => this.getOrders()
    })
  }

}
