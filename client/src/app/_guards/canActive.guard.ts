import { inject } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivateFn, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { Observable, map, take } from "rxjs";
import { AuthService } from "../_services/auth.service";

export const canActivate: CanActivateFn = (
  route: ActivatedRouteSnapshot, state: RouterStateSnapshot
): Observable<boolean| UrlTree> |
  Promise<boolean | UrlTree> |
  boolean | UrlTree => {
    let router = inject(Router);
    return inject(AuthService).currentUser$.pipe(take(1), map(user => {
      const isAuthneticated: boolean = !!user;
      if (isAuthneticated) {
        return true;
      } else {
        return router.createUrlTree(['/auth']);
      }
    }) 
  )
}