import { Component } from '@angular/core';
import { faSquareFacebook, faSquareInstagram } from "@fortawesome/free-brands-svg-icons";

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrl: './footer.component.css'
})
export class FooterComponent {
  faFacebook = faSquareFacebook;
  faInstagram = faSquareInstagram;
  isVisible: IsVisible & { [key: string]: boolean } = {
    info: false,
    pcParts: false,
    desktopPCs: false,
    laptops: false,
    address: false
  };

  onToggle(identifier: string) {
    this.isVisible[identifier] = !this.isVisible[identifier];

    for (const key in this.isVisible) {
        if (key !== identifier) {
            this.isVisible[key] = false;
        }
    }
  }
}

interface IsVisible {
    info: boolean,
    pcParts: boolean,
    desktopPCs: boolean,
    laptops: boolean,
    address: boolean
}
