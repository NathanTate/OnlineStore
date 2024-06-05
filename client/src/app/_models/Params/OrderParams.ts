import { PaginationParams } from "./PaginationParams";

export class OrderParams extends PaginationParams {
  sortBy: string = 'desc';
  sortColumn: string = 'date';
}