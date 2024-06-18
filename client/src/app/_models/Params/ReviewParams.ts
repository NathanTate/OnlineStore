import { PaginationParams } from "./PaginationParams";

export class ReviewParams extends PaginationParams {
  id: number = 0;
  sortBy: string = 'desc';
  sortColumn: string = 'date';
  stars: number = 0;
}