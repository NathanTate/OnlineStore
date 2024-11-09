import { Component, ElementRef, HostListener, OnDestroy, OnInit, ViewChild } from "@angular/core";
import { faFacebook, faInstagram } from "@fortawesome/free-brands-svg-icons";
import { faCartShopping } from "@fortawesome/free-solid-svg-icons";
import { AuthService } from "../_services/auth.service";
import { CartService } from "../_services/cart.service";
import { SubCategoryGroups } from "../_models/Categories";
import { debounceTime, distinctUntilChanged, Subject, Subscription, takeUntil } from "rxjs";
import { CategoryService } from "../_services/category.service";

@Component({
    selector: 'app-header',
    templateUrl: './header.component.html',
    styleUrl: './header.component.css'
})
export class HeaderComponent implements OnInit, OnDestroy{
    @ViewChild('navHeader') navHeader: ElementRef;
    private hoverCategory = new Subject<MouseEvent>();
    private stopHover = true;
    @ViewChild('menuBtn') menuBtn: ElementRef;
    subcategoryGroups: SubCategoryGroups[] = [];
    activeCategoryId: number = -1;
    expandingCategories: {title: string, categoryId: number}[] = [];
    iconFacebook = faFacebook;
    iconInstagram = faInstagram;
    iconCart = faCartShopping;
    isNavExpanded = false;
    private hoverSubscription: Subscription;
    private lastCategoryClicked: HTMLElement;

    constructor(public authService: AuthService, public cartService: CartService, 
      private categoryService: CategoryService) {
        this.initializeExpandingCategories();
    }

    ngOnInit(): void {
        this.hoverSubscription = this.hoverCategory.pipe(
            distinctUntilChanged(),
            debounceTime(300)
        ).subscribe({
            next: (event) => this.stopHover ? null : this.handleCategoryHover(event)
        })
    }

    onClose() {
        this.isNavExpanded = false;
        if (this.lastCategoryClicked){
            const parentLi = this.lastCategoryClicked.closest('.nav-link');
            if (parentLi)
                parentLi.classList.remove('active')
        }
    }

    onLinkClicked(event: Event) {
        const element = event.target as HTMLElement;
        if (element.tagName == 'A') {
            this.isNavExpanded = false;
        }
    }

    onDropdownLinkClick(event: Event) {
        const parentLi = (event.target as HTMLElement).closest('.nav-link');
        if (parentLi) {
          parentLi.classList.remove('active');
        }
    }
    

    @HostListener('document:click', ['$event']) 
        handleClick(event: MouseEvent) {
            if (!this.navHeader) return;
            if(!this.navHeader.nativeElement.contains(event.target) && !this.menuBtn.nativeElement.contains(event.target)) {
                this.isNavExpanded = false;
                if (this.lastCategoryClicked)
                    this.lastCategoryClicked.classList.remove('active')
            }   
        }

    onCategoryHover(event: MouseEvent) {
        this.stopHover = false;
        this.hoverCategory.next(event);
    }

    onCategoryClick(event: MouseEvent) {
        const el = event.target as HTMLElement;
        this.lastCategoryClicked = el;
        if (el.tagName !== 'LI') {
            el.classList.remove('active');
            return;
        }
        this.stopHover = false;
        this.hoverCategory.next(event)
    }

    onCategoryLeave(event: MouseEvent) {
        this.stopHover = true;
        const el = event.target as HTMLElement;
        const categoryId = el.getAttribute('categoryId');
        this.activeCategoryId = categoryId ? +categoryId : -1;;
        el.classList.remove('active');
    }

    handleCategoryHover(event: MouseEvent) {
        const el = event.target as HTMLLIElement;
        const categoryId = el.getAttribute('categoryId');
        if (categoryId === null) return;
        this.activeCategoryId = +categoryId;
        this.getSubcategories(+categoryId)
        el.classList.add('active')
    }

    getSubcategories(categoryId: number) {
        this.categoryService.getSubCategories(categoryId).subscribe({
            next: (subcategoryGroups) => {
                this.subcategoryGroups = subcategoryGroups;
            }                 
        })
    }

    initializeExpandingCategories() {
        this.expandingCategories = [
            {title: 'Laptops', categoryId: 3},
            {title: `Monitors`, categoryId: 2},
            {title: 'Networking Devices', categoryId: 8},
            {title: 'Printers & Scanners', categoryId: 9}
        ]
    }

    ngOnDestroy(): void {
        if (this.hoverSubscription) {
            this.hoverSubscription.unsubscribe();
        }
    }
 
}