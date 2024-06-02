import { AfterViewInit, Component, ElementRef, HostBinding, HostListener, Input, OnInit, ViewChild } from '@angular/core';
import { faArrowLeft, faArrowRight } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-carousel',
  templateUrl: './carousel.component.html',
  styleUrl: './carousel.component.css'
})
export class CarouselComponent implements OnInit, AfterViewInit{
  @ViewChild('content', {static: true}) content: ElementRef;
  @HostBinding('style.cursor') cursor = 'grab';
  @HostBinding('style.user-select') userSelect = 'auto';
  @Input() items = 5;
  _itemsToScroll = 1;
  @Input()
  set itemsToScroll(amount: number) {
    this._itemsToScroll = amount > this.items ? 1 : amount;
  }
  @Input() showControls = true;
  @Input() theme: 'dark' | 'light' = 'light';
  @Input() size: string = 'auto';
  iconNext = faArrowRight;
  iconPrevious = faArrowLeft;
  private isDragging = false;
  startX: number;
  scrollLeft: number;
  carouselInner: HTMLDivElement;

  ngOnInit(): void {
    this.carouselInner = this.content.nativeElement;
  }

  ngAfterViewInit(): void {
    const dynamic = Math.floor(this.carouselInner.offsetWidth / ((<HTMLElement>this.carouselInner.firstChild).offsetWidth + 12));
    if(dynamic < this._itemsToScroll) {
      this._itemsToScroll = dynamic;
    } 
  }

  @HostListener('mouseenter') onMouseEnter() {
    this.cursor = 'grab';
  }

  @HostListener('mousedown', ['$event']) onMouseDown(event: MouseEvent) {
    event.preventDefault();
    this.isDragging = true;
    this.cursor = 'grabbing';
    this.startX = event.pageX - this.carouselInner.getBoundingClientRect().left;
    this.scrollLeft = this.carouselInner.scrollLeft;
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
    this.carouselInner.scrollLeft = this.scrollLeft + (content.offsetWidth + 12) * this._itemsToScroll;
    this.carouselInner.style.scrollBehavior = 'revert'
  }

  showPreviousItem() {
    this.carouselInner.style.scrollBehavior = 'smooth'
    const content = this.carouselInner.firstChild as HTMLElement;
    this.carouselInner.scrollLeft = this.scrollLeft - (content.offsetWidth + 12) * this._itemsToScroll;
    this.carouselInner.style.scrollBehavior = 'revert'
  }
}
