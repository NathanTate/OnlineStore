export interface ProductResponse {
  items: Product[],
  page: number,
  pageSize: number,
  totalCount: number
}

export interface Product {
  id: number;
  name: string;
  description: string;
  productRating: number;
  totalReviews: number;
  brand: Brand;
  originalPrice: number;
  salePrice: number;
  color: string;
  productImages: ProductImage[];
  productSpecifications: ProductSpecification[];
  reviews: Review[];
}

interface Brand {
  id: number;
  brandName: string;
  brandDescription: string;
}

interface ProductImage {
  id: number;
  url: string;
  isMain: boolean;
}

interface ProductSpecification {
  name: string;
  value: string;
}

interface Review {
  id: number;
  pros: string;
  cons: string;
  comment: string;
  rating: Rating;
  productId: number;
}

interface Rating {
  id: number;
  ratingScore: number;
  orderStatus: string;
  userId: string;
}