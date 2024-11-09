import { Component, OnDestroy, OnInit } from '@angular/core';
import { CategoryService } from '../../../_services/category.service';
import { CategoryWithSubGroups } from '../../../_models/Categories';
import { Subscription } from 'rxjs';
import { ModalService } from '../../../_services/modal.service';
import { faAdd, faEdit, faTrash } from '@fortawesome/free-solid-svg-icons';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrl: './categories.component.css'
})
export class CategoriesComponent implements OnInit, OnDestroy {
  categories: CategoryWithSubGroups[];
  categoriesSubscription: Subscription;
  progressMap = new Map<number, number>();
  editMode = false;
  categoryForm: FormGroup;

  iconTrash = faTrash;
  iconEdit = faEdit;
  iconAdd = faAdd;

  constructor(private categoryService: CategoryService, private modalService: ModalService,
    private toastr: ToastrService, private fb: FormBuilder) {}

  ngOnInit(): void {
    this.getCategories();
    this.initializeForm();
  }

  onSubmit() {
    if (!this.categoryForm.valid) return;
    if(!this.editMode) {
      this.categoryService.addCategory(this.categoryForm.value).subscribe({
        next: () => {
          this.getCategories();
          this.toastr.success('category successfully created')
        } 
      });
    } else {
      this.categoryService.updateCategory(this.categoryForm.value).subscribe({
        next: () => {
          this.getCategories();
          this.toastr.success('category successfully updated')
        } 
      });
    }
    this.closeModal();
  }

  getCategories() {
    this.categoryService.getCategories();
    this.categoriesSubscription = this.categoryService.categories$.subscribe({
      next: (categories) => {
        if(categories !== null) {
          this.categories = categories;
          for (let item of categories) {
            this.progressMap.set(item.id, 0);
          }
        }
      }
    })
  }

  deleteCategory(e: number, id: number) {
    this.progressMap.set(id, e / 10);
    const progress = this.progressMap.get(id);
    if(progress !== undefined && progress > 100) {
      this.categoryService.deleteCategory(id).subscribe({
        next: () => {
          this.getCategories();
          this.toastr.success(`Order №${id} was successfully deleted`)
        }
      })
    }
  }

  CreateModal() {
    this.editMode = false;
    this.categoryForm.reset();
    this.modalService.openModal('category');
  }

  EditModal(category: CategoryWithSubGroups) {
    this.editMode = true;
    this.categoryForm.setValue({
      'id': category.id,
      'categoryName': category.categoryName,
      'categoryDescription': category.categoryDescription
    })
    this.modalService.openModal('category');
  }

  closeModal() {
    this.modalService.closeModal('category');
  }

  initializeForm() {
    this.categoryForm = this.fb.nonNullable.group({
      id: [1],
      categoryName: ['', [Validators.required, Validators.maxLength(100)]],
      categoryDescription: ['', Validators.required]
    })
  }

  get categoryName() {
    return this.categoryForm.get('categoryName');
  }

  get categoryDescription() {
    return this.categoryForm.get('categoryDescription');
  }

  get columns(): {column: string, sortable: boolean}[] {
    const keys: {column: string, sortable: boolean}[] = [
    { column: 'Id', sortable: false },
    { column: 'Category Name', sortable: false },
    { column: 'Description', sortable: false },
    ];

    return keys;
  }
  
  ngOnDestroy(): void {
    if (this.categoriesSubscription) {
      this.categoriesSubscription.unsubscribe();
    }
  }
}
