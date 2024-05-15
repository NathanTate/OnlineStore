import { Injectable } from "@angular/core";
import { ProductResponse } from "../_models/Product";
import { HttpClient, HttpParams } from "@angular/common/http";
import { environment } from "../../environments/environment.development";
import { SubCategory } from "../_models/Categories";
import { ProductParams } from "../_models/ProductParams";

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  baseUrl = environment.apiUrl;
  productParams = new ProductParams();

  constructor(private http: HttpClient) {
  }

  getProductParams(): ProductParams {
    return this.productParams;
  }

  getProducts(productParams: ProductParams = this.productParams) {
    let httpParams = new HttpParams();
    for(let key in productParams) {
      if(Object.hasOwn(productParams, key)) {
        const value = productParams[key];

        if(Array.isArray(value)) {
          value.forEach((v: number | string | boolean) => {
            httpParams = httpParams.append(key, v);
          })
        } else {
          httpParams = httpParams.append(key, value);
        }
      }
    }

    return this.http.get<ProductResponse>(this.baseUrl + 'product/getproducts', {params: httpParams});
  }

  getSubCategories(categoryId: number) {
    return this.http.get<SubCategory[]>(this.baseUrl + 'category/getSubCategories/' + categoryId);
  }

}