import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { inject } from "@angular/core";
import { Router } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { Observable, catchError, switchMap, throwError } from "rxjs";
import { AuthService } from "../_services/auth.service";

export class ErrorInterceptor implements HttpInterceptor {
  toastr = inject(ToastrService);
  router = inject(Router);
  authService = inject(AuthService);
  intercept(req: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const user = localStorage.getItem('user');
    
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        if(error) {
          if (error.status == 401 && user) {
            return this.authService.refreshToken().pipe(
              switchMap(() => next.handle(req)),
              catchError((error) => {
                this.authService.logout();
                this.router.navigate(['/auth'])
                throw error;
              })
            )
          }
          if(error.status == 500) {
            this.toastr.error('Server is currently offline')
            throw error;
          } else if (error?.error?.errors) {
            const firstError = Object.values(error.error.errors)[0] as string;
            this.toastr.error(firstError ,error.status.toString())
            throw error;
          } else if (error.status === 404) {
            this.toastr.error(error?.error[0].message, error.status.toString())
            this.router.navigate(['/']);
            throw error;
          } else if (error.status === 400 && (error.error[0]?.message || error.error.message)) {
            const message = error.error.message == undefined ? error?.error[0].message : error.error.message;
            this.toastr.error(message, error.status.toString())
            throw error;
          }
          this.toastr.error(error.error, error.status.toString());
        }
        throw error;
      })
    );
  }
}