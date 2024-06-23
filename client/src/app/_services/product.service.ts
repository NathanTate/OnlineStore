import { Injectable } from "@angular/core";
import { Color, Product, ProductResponse } from "../_models/Product";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../environments/environment.development";
import { SubCategoryGroups } from "../_models/Categories";
import { ProductParams } from "../_models/Params/ProductParams";
import { generateHttpParams } from "../shared/httpParamsHelper";
import { Observable, map, of } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  baseUrl = environment.apiUrl;
  productParams = new ProductParams();
  subcategoriesCache = new Map<number, SubCategoryGroups[]>();

  constructor(private http: HttpClient) {
  }

  getProductParams(): ProductParams {
    return this.productParams;
  }

  addProduct(model: FormData) {
    console.log(model);
    return this.http.post<void>(this.baseUrl + 'product/CreateProduct', model);
  }

  getProducts(productParams: ProductParams = this.productParams) {
    let httpParams = generateHttpParams<ProductParams>(productParams);

    return this.http.get<ProductResponse>(this.baseUrl + 'product/getproducts', {params: httpParams});
  }

  getProduct(id: number) {
    return this.http.get<Product>(this.baseUrl + 'product/getProduct/' + id);
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

  getColors() {
    return this.http.get<Color[]>(this.baseUrl + 'product/getColors');
  }

  deleteProduct(id: number) {
    return this.http.delete<void>(this.baseUrl + 'product/DeleteProduct/' + id);
  }

}