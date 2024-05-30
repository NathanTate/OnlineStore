import { AfterViewInit, Component, ElementRef, HostBinding, HostListener, Input, ViewChild } from '@angular/core';
import { faArrowLeft, faArrowRight } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-carousel',
  templateUrl: './carousel.component.html',
  styleUrl: './carousel.component.css'
})
export class CarouselComponent implements AfterViewInit{
  @ViewChild('content') content: ElementRef;
  @HostBinding('style.cursor') cursor = 'grab';
  @HostBinding('style.user-select') userSelect = 'auto';
  @Input() items = 5;
  @Input() showControls = true;
  @Input() theme: 'dark' | 'light' = 'light';
  @Input() size: string = 'auto';
  iconNext = faArrowRight;
  iconPrevious = faArrowLeft;
  private isDragging = false;
  startX: number;
  scrollLeft: number;
  carouselInner: HTMLDivElement;

  ngAfterViewInit(): void {
    this.carouselInner = this.content.nativeElement;
  }

  @HostListener('mouseenter') onMouseEnter() {
    this.cursor = 'grab';
  }

  @HostListener('mousedown', ['$event']) onMouseDown(event: MouseEvent) {
    event.preventDefault();
    this.isDragging = true;
    this.cursor = 'grabbing';
    this.startX = event.pageX - this.carouselInner.getBoundingClientRect().left;
    console.log(this.startX)
    this.scrollLeft = this.carouselInner.scrollLeft;
    console.log(this.scrollLeft)
  }

  @HostListener('mousemove', ['$event']) onMouseMove(event: MouseEvent) {
    if(!this.isDragging) return;
    this.userSelect = 'none';
    event.preventDefault();

    const x = event.pageX - this.carouselInner.getBoundingClientRect().left;
    const walk = (x - this.startX);
    this.carouselInner.scrollLeft = this.scrollLeft - walk;
  }

  @HostListener('mouseup')
  @HostListener('mouseleave') onMouseUp() {
    this.isDragging = false;
  }

  @HostListener('touchstart', ['$event'])
    onTouchStart(event: TouchEvent) {
    this.isDragging = true;
    const touch = event.touches[0];
    this.startX = touch.pageX - this.carouselInner.getBoundingClientRect().left;
    this.scrollLeft = this.carouselInner.scrollLeft;
  }

  @HostListener('touchmove', ['$event'])
    onTouchMove(event: TouchEvent) {
    if (!this.isDragging) return;
    this.userSelect = 'none';
    event.preventDefault();
    event.stopPropagation();

    const touch = event.touches[0];
    const x = touch.pageX - this.carouselInner.getBoundingClientRect().left;
    const walk = (x - this.startX);
    this.carouselInner.scrollLeft = this.scrollLeft - walk;
  }

  @HostListener('touchend')
    onTouchEnd() {
    this.isDragging = false;
  }


  showNextItem() {
    this.carouselInner.style.scrollBehavior = 'smooth'
    const content = this.carouselInner.firstChild as HTMLElement;
    this.carouselInner.scrollLeft = this.scrollLeft + content.offsetWidth + 12;
    this.carouselInner.style.scrollBehavior = 'revert'
  }

  showPreviousItem() {
    this.carouselInner.style.scrollBehavior = 'smooth'
    const content = this.carouselInner.firstChild as HTMLElement;
    this.carouselInner.scrollLeft = this.scrollLeft - content.offsetWidth - 12;
    this.carouselInner.style.scrollBehavior = 'revert'
  }
}
