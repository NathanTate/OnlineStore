import { inject } from '@angular/core';
import { CanActivateFn, Router, UrlTree } from '@angular/router';
import { AuthService } from '../_services/auth.service';
import { Observable, map, take } from 'rxjs';

export const adminGuard: CanActivateFn = 
(route, state) : Observable<boolean | UrlTree>
| Promise<boolean | UrlTree>
| boolean | UrlTree => {
  let router = inject(Router);
    return inject(AuthService).hasRole(['ADMIN']).pipe(
      map(hasRole => {
        if(hasRole) {
          return true;
        } else {
          return router.createUrlTree(['/auth']);
        }
      })
    )
};
