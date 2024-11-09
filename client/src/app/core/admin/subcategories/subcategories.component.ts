import { Component, OnDestroy, OnInit } from '@angular/core';
import { CategoryService } from '../../../_services/category.service';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ModalService } from '../../../_services/modal.service';
import { CategoryWithSubGroups, SubCategory, SubCategoryGroups } from '../../../_models/Categories';
import { distinctUntilChanged, Subscription } from 'rxjs';
import { faAdd, faEdit } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-subcategories',
  templateUrl: './subcategories.component.html',
  styleUrl: './subcategories.component.css'
})
export class SubcategoriesComponent implements OnInit, OnDestroy {
  editMode: boolean = false;
  subcategoryForm: FormGroup;
  categories: CategoryWithSubGroups[] = [];
  subcategorieGroups: SubCategoryGroups[] = [];
  subcategories: SubCategory[] = [];
  categorySubscription: Subscription;
  iconEdit = faEdit;
  iconAdd = faAdd;

  constructor(private categoryService: CategoryService, private fb: FormBuilder,
    private modalService: ModalService, private toastr: ToastrService) {
    
  }

  ngOnInit(): void {
    this.initializeForm();
  }

  onSubmit() {

  }

  getCategories() {
    this.categoryService.getCategories();
    this.categorySubscription = this.categoryService.categories$.subscribe({
      next: (categories) => {
        if(categories !== null) {
          this.categories = categories;
        }
      }
    })
  }

  initializeForm() {
    this.subcategoryForm = this.fb.group({
      id: [''],
      subcategoryName: ['', Validators.required, Validators.maxLength(100)],
      subcategoryDescription: ['', Validators.required],
      categoryId: ['', Validators.required],
      groupName: ['', [Validators.required, Validators.maxLength(100)]]
    })

    this.categoryId?.valueChanges.pipe(distinctUntilChanged()).subscribe((res: number) => {
      if (res) {
        this.subcategorieGroups = this.categories.find(category => category.id === res)!.subcategoryGroups;
        this.subcategories = this.subcategorieGroups.flatMap(s => s.subcategories);
        this.id?.enable();
      } else {
        this.id?.disable();
      }
    })
  }

  CreateModal() {
    this.editMode = false;
    this.subcategoryForm.reset();
    this.modalService.openModal('subcategory');
  }

  EditModal(subcategory: SubCategory) {
    this.editMode = true;
    this.subcategoryForm.setValue({
      'id': subcategory.id,
      'subcategoryName': subcategory.subCategoryName,
      'subcategoryDescription': subcategory.subCategoryDescription,
      'categoryId': subcategory.categoryId,
      'groupName': subcategory.groupName
    })
    this.modalService.openModal('category');
  }

  ngOnDestroy(): void {
    if (this.categorySubscription) {
      this.categorySubscription.unsubscribe();
    }
  }

  get columns(): {column: string, sortable: boolean}[] {
    const keys: {column: string, sortable: boolean}[] = [
    { column: 'Id', sortable: false },
    { column: 'Subcategory Name', sortable: false },
    { column: 'Description', sortable: false },
    { column: 'Category Id', sortable: false },
    { column: 'Group Name', sortable: false}
    ];

    return keys;
  }

  get id() {
    return this.subcategoryForm.get('id');
  }
  get subcategoryName() {
    return this.subcategoryForm.get('subcategoryName');
  }
  get subcategoryDescription() {
    return this.subcategoryForm.get('subcategoryDescription');
  }
  get categoryId() {
    return this.subcategoryForm.get('categoryId');
  }
  get groupName() {
    return this.subcategoryForm.get('groupName');
  }
}
