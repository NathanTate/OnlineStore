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
          if(error.status == 500) {
            this.toastr.error('Server is currently offline')
            throw error;
          } else if (error?.error?.errors) {
            const firstError = Object.values(error.error.errors)[0] as string;
            this.toastr.error(firstError ,error.status.toString())

            throw error;
          } else if (error.status === 400 && error.error[0]?.message) {
            this.toastr.error(error?.error[0].message, error.status.toString())
            throw error;
          }
          this.toastr.error(error.error, error.status.toString());
        }
        throw error;
      })
    );
  }
}