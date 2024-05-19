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