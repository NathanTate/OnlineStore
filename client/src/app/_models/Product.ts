export interface Product {
  id: number;
  name: string;
  description: string;
  productRating: number;
  totalReviews: number;
  brand: Brand;
  productItems: ProductItem[];
  productImages: ProductImage[];
  productSpecifications: ProductSpecification[];
  reviews: Review[];
}

interface Brand {
  id: number;
  brandName: string;
  brandDescription: string;
}

interface ProductItem {
  originalPrice: number;
  salePrice: number;
  productCode: string;
  color: string;
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