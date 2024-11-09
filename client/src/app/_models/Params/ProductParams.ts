// export interface ProductParams {
//   [key: string]: number | number[] | string[];
//   categoryId: number;
//   subCategories: number[];
//   priceStart: number;
//   priceEnd: number;
//   brands: number[];
//   colors: string[];
// }

import { PaginationParams } from "./PaginationParams";

export class ProductParams extends PaginationParams {
  categoryId: number = 2;
  subCategories: number[] = [];
  priceStart: number = 0;
  priceEnd: number = 0;
  searchTerm: string = '';
  inStock: string = '';
  brands: number[] = [];
  colors: string[] = [];
  sortBy: string = 'ascending';
  sortColumn: string = 'rating';
}

export interface FilterParams {
  subCategories: number[];
  priceStart: number;
  priceEnd: number
  brands: number[];
  colors: string[];
}