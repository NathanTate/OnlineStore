import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-star',
  templateUrl: './star.component.html',
  styleUrl: './star.component.css',
})
export class StarComponent implements OnInit{
  @Input() rating: number = 25;
  @Input() size: string = '30px';
  @Input() disabled: boolean = false;
  selectedStar: number = 0;
  ratingValue = 0;

  constructor() {
  }

  ngOnInit(): void {

  }

  submitRating(rating: number) {
    this.ratingValue = rating;
  }

  onMouseEnter(value: number) {
    this.rating = value * 10;
  }

  onMouseLeave() {
    this.rating = 0;
  }
}
