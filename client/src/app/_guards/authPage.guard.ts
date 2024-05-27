import { inject } from "@angular/core";
import { CanActivateFn, Router, UrlTree } from "@angular/router";
import { Observable, map, take } from "rxjs";
import { AuthService } from "../_services/auth.service";

export const authPageGuard: CanActivateFn = 
(route, active):
Observable<boolean | UrlTree> |
Promise<boolean | UrlTree> |
boolean | UrlTree => {
  const router = inject(Router);
  return inject(AuthService).currentUser$.pipe(take(1), map(user => {
    const isAuthneticated = !!user;
    if(isAuthneticated) {
      return router.createUrlTree(['/'])
    } else {
      return true;
    }
  }))
}