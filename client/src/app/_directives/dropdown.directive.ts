import { Directive, ElementRef, HostBinding, HostListener } from "@angular/core";

@Directive({
  selector: '[appDropdown]'
})
export class DropdownDirective{
  @HostBinding('attr.data-visible') isOpen = false;

  constructor(private elRef: ElementRef<HTMLElement>) {
  }

  @HostListener('document:click', ['$event']) toggleOpen(event: Event) {
    this.isOpen = this.elRef.nativeElement.contains(<HTMLElement>event.target) ? !this.isOpen : false;
  }
}