import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "../../environments/environment.development";
import { Category, CategoryWithSubGroups, SubCategory, SubCategoryGroups } from "../_models/Categories";
import { BehaviorSubject, map, Observable, of } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  baseUrl = environment.apiUrl;
  private categoriesSubject = new BehaviorSubject<CategoryWithSubGroups[] | null>(null);
  categories$ = this.categoriesSubject.asObservable();
  subcategoriesCache = new Map<number, SubCategoryGroups[]>();

  constructor(private http: HttpClient) {}

  addCategory(model: Category) {
    return this.http.post<void>(this.baseUrl + 'category/CreateCategory', model);
  }

  getCategories(): void {
    this.http.get<CategoryWithSubGroups[]>(this.baseUrl + 'category/GetCategories').subscribe({
      next: (categories) => this.categoriesSubject.next(categories)
    })
  }

  getCategory(categoryId: number) {
    return this.http.get<Category>(this.baseUrl + 'category/GetCategory/' + categoryId);
  }

  updateCategory(model: Category) {
    return this.http.put<void>(this.baseUrl + 'category/UpdateCategory', model);
  }

  deleteCategory(categoryId: number) {
    return this.http.delete<void>(this.baseUrl + 'category/DeleteCategory/' + categoryId);
  }


  addSubcategory(model: SubCategory) {
    return this.http.post<void>(this.baseUrl + 'category/CreateCategory', model);
  }

  getSubCategories(categoryId: number): Observable<SubCategoryGroups[]> {
    const subcategoryCache = this.subcategoriesCache.get(categoryId);

    if(subcategoryCache) return of(subcategoryCache);

    return this.http.get<SubCategoryGroups[]>(this.baseUrl + 'category/getSubCategories/' + categoryId).pipe(
      map((subcategoryGroups) => {
       this.subcategoriesCache.set(categoryId, subcategoryGroups);
       return subcategoryGroups;
      })
    );
  }

  getSubCategory(subCategoryId: number) {
    return this.http.get<SubCategory>(this.baseUrl + 'category/GetSubCategory/' + subCategoryId);
  }

  updateSubcategory(model: SubCategory) {
    return this.http.put<void>(this.baseUrl + 'category/UpdateSubCategory', model);
  }

  deleteSubcategory(subcategoryId: number) {
    return this.http.delete<void>(this.baseUrl + 'category/DeleteSubCategory/' + subcategoryId);
  }
}