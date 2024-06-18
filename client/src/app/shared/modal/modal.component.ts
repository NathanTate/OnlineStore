import { Component, HostBinding, HostListener, Input, OnDestroy, OnInit, Renderer2, TemplateRef } from '@angular/core';
import { ModalService } from '../../_services/modal.service';
import { Subscription } from 'rxjs';
import { faXmarkCircle } from '@fortawesome/free-solid-svg-icons';
import { trigger, transition, style, animate } from '@angular/animations';

@Component({
  selector: 'app-modal',
  template: `
  <div *ngIf="isOpen" (mousedown)="onClose()" class="wrapper">
    <div [@enterAnimation] (mousedown)="stopPropagation($event)" class="modal">
      <div class="modal__header">
        <ng-container [ngTemplateOutlet]="headerTemplate || defaultModalHeader"></ng-container>
        <ng-template #defaultModalHeader>
          <div class="modal__title">Your title</div>
        </ng-template>
      </div>

      <ng-container [ngTemplateOutlet]="bodyTemplate || defaultModalBody"></ng-container>
      <ng-template #defaultModalBody>
        <div class="modal__body">Your body goes here...</div>
      </ng-template>

      <ng-container [ngTemplateOutlet]="footerTemplate || defaultModalFooter"></ng-container>
      <ng-template #defaultModalFooter>
      </ng-template>
      <button (click)="onClose()" class="modal__close-button">
        <fa-icon [icon]="iconXmark" size="2x"></fa-icon>
      </button>
    </div>
  </div>
  `,
  styleUrls: ['./modal.component.css'],
  animations: [
    trigger(
      'enterAnimation', [
        transition(':enter', [
          style({ transform: 'scaleY(0.5)', opacity: 0, 'transform-origin': 'top' }),
          animate('200ms', style({ transform: 'scaleY(1)', opacity: 1 }))
        ]),
        transition(':leave', [
          style({ transform: 'scaleY(1)', opacity: 1 }),
          animate('200ms', style({ transform: 'scaleY(0.5)', opacity: 0 }))
        ])
      ]
    )
  ],
})
export class ModalComponent implements OnInit, OnDestroy {
  isOpen = false;
  private subscription: Subscription;
  iconXmark = faXmarkCircle;

  @Input() modalId: string;
  @Input() headerTemplate!: TemplateRef<any>;
  @Input() bodyTemplate!: TemplateRef<any>;
  @Input() footerTemplate!: TemplateRef<any>;

  constructor(private renderer: Renderer2, private modalService: ModalService) {}

  ngOnInit(): void {
    this.subscription = this.modalService.getModalState(this.modalId).subscribe(isOpen => {
      this.isOpen = isOpen;
      if (isOpen) {
        this.renderer.addClass(document.body, 'modal-open');
      } else {
        this.renderer.removeClass(document.body, 'modal-open');
      }
    });
  }

  onClose() {
    this.modalService.closeModal(this.modalId);
  }

  stopPropagation(event: Event) {
    event.stopPropagation();
  }

  @HostListener('document:keyup.escape') onEscape() {
    this.onClose();
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
