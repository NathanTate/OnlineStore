import { Injectable } from "@angular/core";
import { Subject } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ModalService {
  isOpen = new Subject<boolean>();

  closeModal() {
    this.isOpen.next(false);
  }

  openModal() {
    this.isOpen.next(true);
  }
}