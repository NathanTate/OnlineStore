import { AfterViewInit, Component, ElementRef, HostListener, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { faArrowLeft, faArrowRight, faCircle} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-image-slider',
  templateUrl: './image-slider.component.html',
  styleUrl: './image-slider.component.css'
})
export class ImageSliderComponent implements OnInit, OnDestroy, AfterViewInit{
  @ViewChild('blur') blurTag: ElementRef<HTMLAnchorElement>;
  @Input() images: string[];
  @Input() auto: boolean = false;
  @Input() interval: number = 5;
  @Input() showDots: boolean = true;
  @Input() colors: {arrowColor: string, btnColor: string} 
    = {arrowColor: 'blue', btnColor: 'transparent'}
  imageIndex: number = 0;
  intervalId: ReturnType<typeof setInterval>;
  iconPrevious = faArrowLeft;
  iconNext = faArrowRight;
  iconCircle = faCircle;

  constructor() {}

  ngAfterViewInit(): void {
    const img = this.blurTag.nativeElement.querySelector('img');

    if(img && img.complete) {
      this.loaded(this.blurTag.nativeElement)
    }
  }

  loaded(el: HTMLElement) {
    el.classList.add('loaded')
  }


  @HostListener('mouseenter')
  @HostListener('touchstart') onMousedown() {
    if(this.intervalId) {
      clearInterval(this.intervalId)
    }
  }

  @HostListener('mouseleave')
  @HostListener('document:touchend') onMouseup() {
    if (this.auto) {
      clearInterval(this.intervalId)
      this.startInterval();
    }
  }

  ngOnInit(): void {
    this.startInterval();
  }

  showNextSlide() {
    this.imageIndex = this.imageIndex === this.images.length - 1 
      ? 0 : this.imageIndex + 1;
    this.resetInterval();
  }

  showPrevSlide() {
    this.imageIndex = this.imageIndex === 0 
      ? this.images.length - 1 : this.imageIndex -1;
    this.resetInterval();
  }

  startInterval() {
    if (this.auto) {
      this.intervalId = setInterval(this.showNextSlide.bind(this), this.interval * 1000);
    }
  }

  resetInterval() {
    if (this.auto && this.intervalId) {
      clearInterval(this.intervalId);
      this.startInterval();
    }
  }

  ngOnDestroy(): void {
    if(this.intervalId) {
      clearInterval(this.intervalId);
    }
  }
}
