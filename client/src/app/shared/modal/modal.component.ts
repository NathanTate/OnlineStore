import { Component, HostListener, Input, OnDestroy, OnInit, TemplateRef, inject } from '@angular/core';
import { ModalService } from '../../_services/modal.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-modal',
  template: `
  <div *ngIf="isOpen" (click)="onClose()" class="wrapper">
    <div clickStopPropogation class="modal">
      <ng-container [ngTemplateOutlet]="headerTemplate || defeaultModalHeader"></ng-container>
      <ng-template #defeaultModalHeader>
        <div class="modal__title">Your title</div>
      </ng-template>

      <ng-container [ngTemplateOutlet]="bodyTemplate || defaultModalBody"></ng-container>
      <ng-template #defaultModalBody>
        <div class="modal__body">Your body goes here...</div>
      </ng-template>

      <ng-container [ngTemplateOutlet]="footerTemplate || defaultModalFooter"]></ng-container>
      <ng-template #defaultModalFooter>
      </ng-template>
    </div>
  </div>
  `,
  styleUrl: './modal.component.css'
})
export class ModalComponent implements OnInit, OnDestroy {
  isOpen = true;
  private subscription: Subscription;
  private modalService = inject(ModalService)

  @Input() headerTemplate!: TemplateRef<any>;
  @Input() bodyTemplate!: TemplateRef<any>;
  @Input() footerTemplate!: TemplateRef<any>;
  
  
  
  ngOnInit(): void {
    this.subscription = this.modalService.isOpen.subscribe({
      next: (value: boolean) => {
        this.isOpen = value;
      }
    })
  }

  onClose() {
    this.isOpen = false;
  }

  @HostListener('document:keyup.escape') onEscape() {
    this.isOpen = false;
  } 

  ngOnDestroy(): void {
    if(this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
