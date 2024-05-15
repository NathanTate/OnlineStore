import { Component } from '@angular/core';
import { faHeart, faStar, faCar } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrl: './about.component.css'
})
export class AboutComponent {
  heartIcon = faHeart;
  starIcon = faStar;
  carIcon = faCar;
}
