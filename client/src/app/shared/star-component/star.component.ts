import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-star',
  templateUrl: './star.component.html',
  styleUrl: './star.component.css',
})
export class StarComponent implements OnInit{
  @Input() rating: number = 0;
  @Input() size: string = '30px';
  @Input() disabled: boolean = false;
  selectedStar: number = 0;
  isSelected = false;
  ratingValue = 0;
  @Output() ratingEmit = new EventEmitter<number>();

  constructor() {
  }

  ngOnInit(): void {

  }

  submitRating(rating: number) {
    this.rating = this.ratingValue = rating * 10;
    this.ratingEmit.emit(this.ratingValue);
    this.isSelected = true;
  }

  onMouseEnter(value: number) {
    this.rating = value * 10;
  }

  onMouseLeave() {
    if(!this.isSelected) {
      this.rating = 0;
    } else {
      this.rating = this.ratingValue;
    }
  }
}
