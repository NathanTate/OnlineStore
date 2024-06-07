import { Directive, HostListener } from "@angular/core";

@Directive({
  selector: '[clickStopPropogation]'
})
export class clickStopPropogation {
  constructor () {}

  @HostListener('click', ['$event']) onClick(event: Event): void {
    event.stopPropagation();
  }
}