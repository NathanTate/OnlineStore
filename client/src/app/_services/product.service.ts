import { Injectable } from "@angular/core";
import { Color, Product, ProductRequest, ProductResponse } from "../_models/Product";
import { HttpClient, HttpParams } from "@angular/common/http";
import { environment } from "../../environments/environment.development";
import { SubCategory } from "../_models/Categories";
import { ProductParams } from "../_models/Params/ProductParams";
import { generateHttpParams } from "../shared/httpParamsHelper";

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

  getSubCategories(categoryId: number) {
    return this.http.get<SubCategory[]>(this.baseUrl + 'category/getSubCategories/' + categoryId);
  }

  getColors() {
    return this.http.get<Color[]>(this.baseUrl + 'product/getColors');
  }

}