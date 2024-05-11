import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-ad-banner',
  templateUrl: './ad-banner.component.html',
  styleUrl: './ad-banner.component.css'
})
export class AdBannerComponent {
  @Input() maxHeight: string = 'auto';
  @Input() imageUrl: string;
}
