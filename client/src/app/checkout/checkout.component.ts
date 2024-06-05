import { Component, OnDestroy, OnInit } from '@angular/core';
import { CartResponse } from '../_models/Cart';
import { CartService } from '../_services/cart.service';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { OrderService } from '../_services/order.service';
import { OrderCheckoutRequest } from '../_models/Order';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.css'
})
export class CheckoutComponent implements OnInit, OnDestroy{
  cart: CartResponse;
  cartSubscription: Subscription;
  checkoutForm: FormGroup;

  constructor(private cartService: CartService, private fb: FormBuilder, 
    private toastr: ToastrService, private orderService: OrderService) {}

  ngOnInit(): void {
    this.getCart();
    this.initializeForm();
  }

  onSumbit() {
    if(!this.checkoutForm.valid) {
      this.toastr.error('Submission of invalid form')
      return;
    }

    const model: OrderCheckoutRequest = {cartResponse: this.cart, ...this.checkoutForm.value}
  
    this.orderService.checkout(model).subscribe({
      next: (redirectUrl) => {
        this.toastr.success('Checkout is successfull')
        if(redirectUrl) {
          window.location.href = redirectUrl;
        }
      },
      complete: () => {
        this.checkoutForm.reset();
        // this.cartService.removeFromCart(0, true).subscribe();
      }
    });
  }

  getCart() {
    this.cartSubscription = this.cartService.cart$.subscribe({
      next: (cart: CartResponse | null) => {
        if(cart !== null) {
          this.cart = cart
        }
      }
    });
  }

  private initializeForm() {
    this.checkoutForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      firstName: ['', [Validators.required, length(2, 50)]],
      lastName: ['', [Validators.required, length(2, 50)]],
      address: this.fb.group({
        country: ['', [Validators.required, length(2, 100)]],
        state: ['', [Validators.required, length(2, 100)]],
        city: ['', [Validators.required, length(2, 100)]],
        street: ['', [Validators.required, length(2, 100)]],
        zipCode: ['', [Validators.required, length(2, 100)]],
        houseNumber: ['', [Validators.required, length(2, 50)]]
      }),
      phone: ['+', [Validators.required, 
        Validators.pattern(/^\+(?:9[976]\d|8[987530]\d|6[987]\d|5[90]\d|42\d|3[875]\d|2[98654321]\d|9[8543210]|8[6421]|6[6543210]|5[87654321]|4[987654310]|3[9643210]|2[70]|7|1)\d{1,14}$/), 
        length(10, 13)]]
    })
  }

  get email() {
    return this.checkoutForm.get('email');
  }
  get firstName() {
    return this.checkoutForm.get('firstName');
  }
  get lastName() {
    return this.checkoutForm.get('lastName');
  }
  get country() {
    return this.checkoutForm.get('address.country');
  }
  get state() {
    return this.checkoutForm.get('address.state');
  }
  get city() {
    return this.checkoutForm.get('address.city');
  }
  get street() {
    return this.checkoutForm.get('address.street');
  }
  get zipCode() {
    return this.checkoutForm.get('address.zipCode');
  }
  get houseNumber() {
    return this.checkoutForm.get('address.houseNumber');
  }
  get phone() {
    return this.checkoutForm.get('phone');
  }

  ngOnDestroy(): void {
    if(this.cartSubscription) {
      this.cartSubscription.unsubscribe();
    }
  }
}

export const length = (min: number, max: number): ValidatorFn => {
  return (control: AbstractControl) : ValidationErrors | null => {
    const value = control.value;
    if(value == undefined || value == null) {
      return null;
    }
    return value.length < min || value.length > max 
    ? {'length': value} : null;
  }
}
