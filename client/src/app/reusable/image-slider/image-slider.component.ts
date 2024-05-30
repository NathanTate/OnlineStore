import { Component, HostListener, Input, OnDestroy, OnInit } from '@angular/core';
import { ProductImage } from '../../_models/Product';
import { faArrowLeft, faArrowRight, faCircle} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-image-slider',
  templateUrl: './image-slider.component.html',
  styleUrl: './image-slider.component.css'
})
export class ImageSliderComponent implements OnInit, OnDestroy{
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
