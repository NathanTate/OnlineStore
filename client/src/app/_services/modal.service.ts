import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ModalService {
  private modalStates: { [key: string]: BehaviorSubject<boolean> } = {};

  getModalState(modalId: string) {
    if (!this.modalStates[modalId]) {
      this.modalStates[modalId] = new BehaviorSubject<boolean>(false);
    }
    return this.modalStates[modalId].asObservable();
  }

  openModal(modalId: string) {
    if (!this.modalStates[modalId]) {
      this.modalStates[modalId] = new BehaviorSubject<boolean>(false);
    }
    this.modalStates[modalId].next(true);
  }

  closeModal(modalId: string) {
    if (this.modalStates[modalId]) {
      this.modalStates[modalId].next(false);
    }
  }
}
