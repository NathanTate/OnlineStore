import { Component, ElementRef, EventEmitter, Input, Output, QueryList, Renderer2, ViewChildren } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { faSort, faTrash } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-datatable',
  templateUrl: './datatable.component.html',
  styleUrl: './datatable.component.css'
})
export class DatatableComponent {
  @ViewChildren('statusBtn') statusButtons: QueryList<ElementRef<HTMLButtonElement>>
  @Input() length: number = 0;
  @Input() columns: { column: string, sortable: boolean }[] = [];
  @Input() actions: any[] = [];
  @Input() totalCount: number = 0;
  @Input() pageSize: number = 10;
  @Input() currentPage: number = 1;
  @Input() filterStatuses: string[] = [];
  @Output() sort = new EventEmitter<string>();
  @Output() filterStatus = new EventEmitter<string>();
  @Output() filter = new EventEmitter<string>();
  @Output() pageChanged = new EventEmitter<number>();

  iconSort = faSort;
  iconTrash = faTrash;

  searchForm: FormGroup = this.fb.nonNullable.group({
    searchTerm: '',
  });

  constructor(private fb: FormBuilder, private renderer: Renderer2) {}

  onSort(column: string) {
    this.sort.emit(column);
  }

  onFilterStatus(event: Event, status: string) {
    this.statusButtons.forEach(button => {
      button.nativeElement.classList.remove('active')
    })
    this.renderer.addClass(event.target, 'active');
    this.filterStatus.emit(status);
  }

  onSearch() {
    const searchTerm = this.searchForm.value.searchTerm ?? '';
    this.filter.emit(searchTerm);
  }

  onPageChange(page: number) {
    this.pageChanged.emit(page);
  }
}
