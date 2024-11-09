import { Injectable } from "@angular/core";
import { Color, Product, ProductRequest, ProductResponse, SetMainPhotoRequest } from "../_models/Product";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../environments/environment.development";
import { ProductParams } from "../_models/Params/ProductParams";
import { generateHttpParams } from "../shared/httpParamsHelper";

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  baseUrl = environment.apiUrl + 'product/';
  productParams = new ProductParams();

  constructor(private http: HttpClient) {
  }

  getProductParams(): ProductParams {
    return this.productParams;
  }

  createPlaceholder() {
    return this.http.post<Product>(this.baseUrl + 'CreatePlaceholder', {});
  }

  updateProduct(model: ProductRequest) {
    return this.http.put<void>(this.baseUrl + 'UpdateProduct', model);
  }

  updatePhotos(model: FormData) {
    return this.http.put<void>(this.baseUrl + 'UpdatePhotos', model);
  }

  setMainPhoto(model: SetMainPhotoRequest) {
    return this.http.put<void>(this.baseUrl + 'SetMainPhoto', model);
  }

  getProducts(productParams: ProductParams = this.productParams) {
    let httpParams = generateHttpParams<ProductParams>(productParams);

    return this.http.get<ProductResponse>(this.baseUrl + 'getproducts/', {params: httpParams});
  }

  getProduct(id: number) {
    return this.http.get<Product>(this.baseUrl + 'getProduct/' + id);
  }

  getColors() {
    return this.http.get<Color[]>(this.baseUrl + 'getColors');
  }

  deleteProduct(id: number) {
    return this.http.delete<void>(this.baseUrl + 'DeleteProduct/' + id);
  }

}