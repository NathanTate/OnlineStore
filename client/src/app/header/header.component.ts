import { AfterViewInit, Component, ElementRef, HostListener, OnInit, Renderer2, ViewChild } from "@angular/core";
import { faSquareFacebook, faSquareInstagram } from "@fortawesome/free-brands-svg-icons";
import { faMagnifyingGlass, faCartShopping, faUser, faSignOut, faTruckFast } from "@fortawesome/free-solid-svg-icons";
import { AuthService } from "../_services/auth.service";
import { CartService } from "../_services/cart.service";
import { ProductService } from "../_services/product.service";
import { SubCategoryGroups } from "../_models/Categories";

@Component({
    selector: 'app-header',
    templateUrl: './header.component.html',
    styleUrl: './header.component.css'
})
export class HeaderComponent implements OnInit, AfterViewInit{
    navListEl: HTMLUListElement;
    @ViewChild('navList') 
        set navListRef(elRef: ElementRef) {
            this.navListEl = elRef.nativeElement;
        }
        subcategoryGroups: SubCategoryGroups[] = [];
    faFacebook = faSquareFacebook;
    faInstagram = faSquareInstagram;
    faSearch = faMagnifyingGlass;
    faCart = faCartShopping;
    faUser = faUser;
    iconTruck = faTruckFast;
    faSignOut = faSignOut
    isVisible = false;
    isMenuVisible = false;
    timeoutId: ReturnType<typeof setTimeout>
    isMouseInside = false;
    clickedWithin = false;
    dropdownExpanded: boolean = false;

    constructor(public authService: AuthService, private render: Renderer2, 
        public cartService: CartService, private productService: ProductService) {}

    ngOnInit(): void {
    }

    onHide() {
        console.log(this.clickedWithin)
        this.clickedWithin = true;
    }

    ngAfterViewInit(): void {
        const liElements = this.navListEl.children
        
        for (let i = 0; i < liElements.length; i++) {
            const li = liElements[i];
            const attributeValue = li.getAttribute('categoryId');
            if(attributeValue === null) continue;
            const categoryId = +attributeValue;
            this.render.listen(li, 'mouseenter', () => {
                if(this.isMouseInside) return;
                this.isMouseInside = true
                this.clickedWithin = false;
                this.timeoutId = setTimeout(() => this.getSubcategories(categoryId), 300)}
            )
            this.render.listen(li, 'click', () => {this.dropdownExpanded = true, li.classList.toggle('active')})
            this.render.listen(li, 'mouseleave', () => { 
                this.isMouseInside = false;
                if(this.timeoutId) {
                    clearTimeout(this.timeoutId)
                }
            })
        }
    
    }

    stopPropagation(event: Event) {
        event.stopPropagation();
      }

    @HostListener('document:click', ['$event']) onGlobalClick(event: MouseEvent) {
        if(!this.navListEl) return;
        if(!this.navListEl.contains(event.target as Node)) {
          this.dropdownExpanded = false;
        }
    }

    getSubcategories(categoryId: number) {
        this.productService.getSubCategories(categoryId).subscribe({
            next: (subcategoryGroups) => {this.subcategoryGroups = subcategoryGroups}
            
        })
    }

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
            this.render.removeClass(document.body, 'modal-open');
        }
    }


    
}