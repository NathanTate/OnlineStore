import { HttpParams } from "@angular/common/http"

export const generateHttpParams = <T extends object>(params: T): HttpParams => {
  let httpParams = new HttpParams();
    for(let key in params) {
      if(Object.hasOwn(params, key)) {
        const value = params[key];

        if(Array.isArray(value)) {
          value.forEach((v: number | string | boolean) => {
            httpParams = httpParams.append(key, v);
          })
        } else {
          httpParams = httpParams.append(key, String(value));
        }
      }
    }
  return httpParams;
}