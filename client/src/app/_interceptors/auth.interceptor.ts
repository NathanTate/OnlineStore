import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Observable, switchMap, take } from "rxjs";
import { AuthService } from "../_services/auth.service";
import { inject } from "@angular/core";

export class AuthInterceptor implements HttpInterceptor {
  private authService = inject(AuthService);

  intercept(req: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return this.authService.currentUser$.pipe(take(1),
    switchMap(user => {
      if(!user) return next.handle(req);
      const modifiedRequest = req.clone({
        headers: req.headers.append('Authorization', 'Bearer' + user.token)
      })
      return next.handle(modifiedRequest)
    }))
  }
}