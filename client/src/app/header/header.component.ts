import { Component, Renderer2 } from "@angular/core";
import { faSquareFacebook, faSquareInstagram } from "@fortawesome/free-brands-svg-icons";
import { faMagnifyingGlass, faCartShopping, faUser, faSignOut } from "@fortawesome/free-solid-svg-icons";
import { AuthService } from "../_services/auth.service";

@Component({
    selector: 'app-header',
    templateUrl: './header.component.html',
    styleUrl: './header.component.css'
})
export class HeaderComponent {
    faFacebook = faSquareFacebook;
    faInstagram = faSquareInstagram;
    faSearch = faMagnifyingGlass;
    faCart = faCartShopping;
    faUser = faUser;
    faSignOut = faSignOut
    isVisible = false;
    isMenuVisible = false;

    constructor(public authService: AuthService, private render: Renderer2) {}

    onToggle() {
        this.isVisible = !this.isVisible;
        if(this.isVisible) {
            this.render.addClass(document.body, 'modal-open');
            return;
        }
        this.render.removeClass(document.body, 'modal-open');
    }

    toggleUserMenu() {
        this.isMenuVisible = !this.isMenuVisible
    }

    closeMenu() {
        this.isMenuVisible = false;
    }

    onResize() {
        if(window.innerWidth > 1280) {
            this.isVisible = false;
        }
    }
}