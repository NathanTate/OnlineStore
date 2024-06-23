import { Component, OnDestroy, OnInit } from '@angular/core';
import { OrderService } from '../_services/order.service';
import { ActivatedRoute, Params } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-validate-order',
  templateUrl: './validate-order.component.html',
  styleUrl: './validate-order.component.css'
})
export class ValidateOrderComponent implements OnInit, OnDestroy {
  subscription: Subscription;
  isLoading = false;
  invalidOrderIdMessage: string = ''

  constructor(private orderService: OrderService, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.subscription = this.route.params.subscribe({
      next: (params: Params) => {
        const orderId = +params['id'];
        if(!isNaN(orderId)) {
          this.isLoading = true;
          this.orderService.validateStripeSession(orderId).subscribe({
            error: (error) => {
              this.invalidOrderIdMessage = error.error[0].message
              this.isLoading = false;
            }, 
            complete: () => this.isLoading = false
          });
          this.invalidOrderIdMessage = '';
          return;
        }
        this.invalidOrderIdMessage = 'Invalid Order Id'

      }
    })
  }

  ngOnDestroy(): void {
    if(this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
