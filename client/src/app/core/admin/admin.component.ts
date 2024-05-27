import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.css'
})
export class AdminComponent implements OnInit{
  // tabs = ['Create Product', 'Edit Products', 'Manage Orders'];

  tabs = new Map();
  tabKeys: string[] = [];

  constructor(private router: Router, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.tabs.set('Create Product', 'product');
    this.tabs.set('Edit Products', 'edit');
    this.tabs.set('Manage Orders', 'orders');
    this.tabKeys = Array.from(this.tabs.keys());
  }

  onTabChanged(tab: string) {
    this.router.navigate([this.tabs.get(tab)], {relativeTo: this.route})
  }
}
