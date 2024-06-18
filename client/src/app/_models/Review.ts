export interface Review {
  id: number;
  pros: string;
  cons: string;
  comment: string;
  ratingScore: number;
  orderStatus: string;
  member: Member;
  createdAt: Date;
  productId: number;
}

export interface Member {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
}

export interface ReviewResponse {
  items: Review[],
  page: number,
  pageSize: number,
  totalCount: number
}

export interface ReviewRequest {
  pros: string,
  cons: string,
  comment: string,
  ratingScore: number;
  orderStatus?: string;
  productId: number;
}