import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrl: './table.component.css'
})
export class TableComponent<T extends object> {
  @Input() items: T[];

  get columns() : Array<keyof T> {
    return Object.keys(this.items[0]) as Array<keyof T>;
  }
}
