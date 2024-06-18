import { Review } from "./Review";

export interface ProductResponse {
  items: Product[],
  page: number,
  pageSize: number,
  totalCount: number
}

export interface ProductRequest {
  name: string;
  originalPrice: number;
  salePrice: number;
  description: string;
  subCategoryId: number;
  categoryId: number;
  brandId: number;
  isMainImage: boolean;
  colors: Color[];
  productSpecifications: ProductSpecification[];
  ProductImages: FileList;
}

export interface Product {
  [key: string]: any;
  
  id: number;
  name: string;
  description: string;
  productRating: number;
  totalReviews: number;
  brand: Brand;
  originalPrice: number;
  salePrice: number;
  colors: Color[];
  productImages: ProductImage[];
  productSpecifications: ProductSpecification[];
  reviews: Review[];
}

interface Brand {
  id: number;
  brandName: string;
  brandDescription: string;
}

export interface ProductImage {
  id: number;
  url: string;
  isMain: boolean;
}

interface ProductSpecification {
  name: string;
  value: string;
}


export interface Color {
  id: number;
  value: string;
}