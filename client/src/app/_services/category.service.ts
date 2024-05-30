import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "../../environments/environment.development";
import { Category, SubCategory } from "../_models/Categories";
import { BehaviorSubject } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  baseUrl = environment.apiUrl;
  private categoriesSubject = new BehaviorSubject<Category[] | null>(null);
  categories$ = this.categoriesSubject.asObservable();

  constructor(private http: HttpClient) {}

  getCategories(): void {
    this.http.get<Category[]>(this.baseUrl + 'category/GetCategories').subscribe({
      next: (categories) => this.categoriesSubject.next(categories)
    })
  }

  getCategory(categoryId: number) {
    return this.http.get<Category>(this.baseUrl + 'category/GetCategory/' + categoryId);
  }

  getSubCategories(categoryId: number) {
    return this.http.get<SubCategory[]>(this.baseUrl + 'category/GetSubCategories/' + categoryId);
  }

  getSubCategory(subCategoryId: number) {
    return this.http.get<SubCategory>(this.baseUrl + 'category/GetSubCategory/' + subCategoryId);
  }
}