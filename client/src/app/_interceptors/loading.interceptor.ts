import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { inject } from "@angular/core";
import { Observable, delay, finalize, tap } from "rxjs";
import { LoadingSpinnerService } from "../_services/loading-spinner.service";

export class LoadingInterceptor implements HttpInterceptor{
  spinnerService = inject(LoadingSpinnerService);

  intercept(req: HttpRequest<unknown>, next: HttpHandler) : Observable<HttpEvent<unknown>> {
    this.spinnerService.start();

    return next.handle(req).pipe(
      finalize(() => {
        this.spinnerService.stop();
      })
    )
  }
}