// export interface ProductParams {
//   [key: string]: number | number[] | string[];
//   categoryId: number;
//   subCategories: number[];
//   priceStart: number;
//   priceEnd: number;
//   brands: number[];
//   colors: string[];
// }

export class ProductParams {
  [key: string]: number | number[] | string[];
  categoryId: number = 2;
  subCategories: number[] = [];
  priceStart: number = 0;
  priceEnd: number = 0;
  brands: number[] = [];
  colors: string[] = [];
}