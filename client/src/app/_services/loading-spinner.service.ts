import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class LoadingSpinnerService {
  private spinnerSubject = new BehaviorSubject<boolean>(false);
  public readonly spinner$ = this.spinnerSubject.asObservable();

  constructor() {}

  start() {
    this.spinnerSubject.next(true);
  }

  stop() {
    this.spinnerSubject.next(false);
  }
}