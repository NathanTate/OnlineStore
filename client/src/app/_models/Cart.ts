import { Product } from "./Product";

export interface CartResponse {
  cartHeader: CartHeaderResponse;
  cartDetails: CartDetailResponse[];
}

export interface CartHeaderResponse {
  id: number;
  userId: number;
  total: number;
  discount: number;
  couponCode: string;
}

export interface CartDetailResponse {
  id: number;
  cartHeaderId: number;
  product: Product;
  count: number;
}

export interface CartDetailRequest {
  cartHeaderId?: number;
  productId: number;
  count: number;
}