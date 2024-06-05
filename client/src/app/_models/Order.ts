import { AddressRequest } from "./Address";
import { CartResponse } from "./Cart";

export interface OrderCheckoutRequest {
  cartResponse: CartResponse;
  firstName: string,
  lastName: string,
  email: string,
  phone: string,
  address: AddressRequest
}

export interface OrderDetail {
  id: number;
  orderHeaderId: number;
  productId: number;
  count: number;
  productName: string;
  productPrice: number;
}

export interface OrderHeader {
  id: number;
  userId: string;
  orderTotal: number;
  couponCode: string;
  discount: number;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  addressId: number;
  orderStatus: string;
  orderDate: Date;
  stripeSessionId: string;
  paymentIntentId: string;
  orderDetails: OrderDetail[];
}

export interface OrderResponse {
  items: OrderHeader[],
  page: number;
  pageSize: number,
  totalCount: number
}
