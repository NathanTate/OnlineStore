import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-read-more-less',
  templateUrl: './read-more-less.component.html',
  styleUrl: './read-more-less.component.css'
})
export class ReadMoreLessComponent {
  @Input() maxLength: number = 350;
  @Input() content: string = '';
  showMore: boolean = false;

  toggleShowMore() {
    this.showMore = !this.showMore;
  }
}
