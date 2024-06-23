import { PaginationParams } from "./PaginationParams";

export class OrderParams extends PaginationParams {
  sortBy: string = 'asc';
  sortColumn: string = 'id';
  searchTerm: string = '';
  orderStatus: string = '';
}