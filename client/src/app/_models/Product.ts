import { Review } from "./Review";

export interface ProductResponse {
  items: Product[],
  page: number,
  pageSize: number,
  totalCount: number
}

export interface ProductRequest {
  id: number;
  name: string;
  originalPrice: number;
  salePrice: number;
  quantity: number;
  description: string;
  subCategoryId: number;
  categoryId: number;
  brandId: number;
  isCreate: boolean;
  colors: ColorRequest[];
  productSpecifications: ProductSpecification[];
}

export interface PhotoUpdateRequest {
  itemId: number;
  photoCollection: FileList;
  idsToRemove: number[];
}

export interface SetMainPhotoRequest {
  itemId: number;
  photoId: number;
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
  subCategoryId: number;
  categoryId: number;
  salePrice: number;
  quantity: number;
  colors: Color[];
  productImages: ProductImage[];
  mainImageUrl: string;
  productSpecifications: ProductSpecification[];
  reviews: Review[];
}

export interface Brand {
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

interface ColorRequest {
  value: string;
}

export interface Color {
  id: number;
  value: string;
}