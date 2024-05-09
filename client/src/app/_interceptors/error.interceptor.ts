import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { inject } from "@angular/core";
import { ToastrService } from "ngx-toastr";
import { Observable, catchError } from "rxjs";

export class ErrorInterceptor implements HttpInterceptor {
  toastr = inject(ToastrService);
  intercept(req: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        if(error) {
          if(error.status == 0) {
            this.toastr.error('Server is currently offline')
            throw error;
          }
          this.toastr.error(error.error, error.status.toString());
        }
        throw error;
      })
    );
  }
}