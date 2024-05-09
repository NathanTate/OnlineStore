import { Injectable } from "@angular/core";
import { Product } from "../_models/Product";
import { HttpClient, HttpParams } from "@angular/common/http";
import { environment } from "../../environments/environment.development";
import { SubCategory } from "../_models/Categories";
import { ProductParams } from "../_models/ProductParams";

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  readonly products: Product[];
  baseUrl = environment.apiUrl;
  filterParams = new ProductParams();

  constructor(private http: HttpClient) {
  }

  getProducts(filterParams: ProductParams = this.filterParams) {
    let httpParams = new HttpParams();
    for(let key in filterParams) {
      if(Object.hasOwn(filterParams, key)) {
        const value = filterParams[key];

        if(Array.isArray(value)) {
          value.forEach((v: number | string | boolean) => {
            httpParams = httpParams.append(key, v);
          })
        } else {
          httpParams = httpParams.append(key, value);
        }
      }
    }

    return this.http.get<Product[]>(this.baseUrl + 'product/getproducts', {params: httpParams});
  }

  getSubCategories(categoryId: number) {
    return this.http.get<SubCategory[]>(this.baseUrl + 'category/getSubCategories/' + categoryId);
  }

}