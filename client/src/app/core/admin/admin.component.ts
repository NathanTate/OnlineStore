import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.css'
})
export class AdminComponent implements OnInit{
  activeTab: string = '';

  tabs = new Map();
  tabKeys: string[] = [];

  constructor(private router: Router, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.tabs.set('Create Product', 'product');
    this.tabs.set('Manage Products', 'manage');
    this.tabs.set('Manage Orders', 'orders');
    this.tabs.set('Manage Categories', 'categories');
    this.tabs.set('Manage SubCategories', 'subcategories');
    this.tabs.set('Manage Brands', 'brands');
    this.tabKeys = Array.from(this.tabs.keys());
    const path = this.route.snapshot.firstChild?.routeConfig?.path;
    for(let [key, value] of this.tabs.entries()) {
      this.activeTab = path === value ? key : this.activeTab;
    }
  }

  onTabChanged(tab: string) {
    this.router.navigate([this.tabs.get(tab)], {relativeTo: this.route})
  }
}
