import { AfterViewInit, Component, ElementRef, Input, ViewChild } from '@angular/core';
import { Testimonial } from '../_models/Testimonial';

@Component({
  selector: 'app-testimonial',
  templateUrl: './testimonial.component.html',
  styleUrl: './testimonial.component.css'
})
export class TestimonialComponent implements AfterViewInit{
  @ViewChild('testimonial') testimonial: ElementRef;
  testimonialEl: HTMLDivElement;
  @Input() testimonials: Testimonial[] = this.getDefaultTestimonials();
  index = 0;

  ngAfterViewInit(): void {
    this.testimonialEl = this.testimonial.nativeElement;
  }

  onChange(i: number) {
    this.index = i;
    this.testimonialEl.scrollLeft = this.testimonialEl.offsetWidth * i;
  }

  onScroll() {
    const scrollLeft = this.testimonialEl.scrollLeft;
    const childWidth = this.testimonialEl.offsetWidth;
    this.index = Math.round(scrollLeft / childWidth);
  }

  getDefaultTestimonials(): Testimonial[] {
    let defaultTestimonials: Testimonial[] = [];
     
    for (let i = 0; i < 4; i++) {
      defaultTestimonials.push({message: `My first order arrived today in perfect condition. From the time I sent a
      question about the item to making the purchase, to the shipping and now
      the delivery, your company, Tecs, has stayed in touch. Such great service.
      I look forward to shopping on your site in the future and would highly
      recommend it.`, author: 'Tama Brown'})
    } 
    // defaultTestimonials.push({message: `My first order arrived today in perfect condition. From the time I sent a
    //   question about the item to making the purchase, to the shipping and now
    //   the delivery, your company, Tecs, has stayed in touch. Such great service.
    //   I look forward to shopping on your site in the future and would highly
    //   recommend it.`, author: 'Tama Brown'})
    //   defaultTestimonials.push({message: `Such great service.
    //   I look forward to shopping on your site in the future and would highly
    //   recommend it.`, author: 'Dicdddk'})
    //   defaultTestimonials.push({message: `Arrived today in perfect condition. From the time I sent a
    //   question about the item to making the purchase,  Such great service.
    //   I look forward to shopping on your site in the future and would highly
    //   recommend it.`, author: 'Grip'})

    return defaultTestimonials;
  }
}
