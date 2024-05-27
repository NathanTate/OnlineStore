import { Component, Input } from '@angular/core';
import { ProductImage } from '../../_models/Product';
import { faArrowLeft, faArrowRight, faCircle} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-image-slider',
  templateUrl: './image-slider.component.html',
  styleUrl: './image-slider.component.css'
})
export class ImageSliderComponent {
  @Input() images: ProductImage[];
  imageIndex: number = 0;
  iconPreviuos = faArrowLeft;
  iconNext = faArrowRight;
  iconCircle = faCircle;

  constructor() {}

  showNextSlide() {
    this.imageIndex = this.imageIndex === this.images.length - 1 
      ? 0 : this.imageIndex + 1;
  }

  showPrevSlide() {
    this.imageIndex = this.imageIndex === 0 
      ? this.images.length - 1 : this.imageIndex -1;
  }
}
