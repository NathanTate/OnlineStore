import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CategoryService } from '../../../../_services/category.service';
import { ProductService } from '../../../../_services/product.service';
import { CategoryWithSubGroups, SubCategoryGroups } from '../../../../_models/Categories';
import { distinctUntilChanged, Subscription } from 'rxjs';
import { Product } from '../../../../_models/Product';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-product-form',
  templateUrl: './product-form.component.html',
  styleUrl: './product-form.component.css'
})
export class ProductFormComponent implements OnInit, OnDestroy{
  @Input() product: Product;
  @Input() isEdit: boolean = false;
  productForm: FormGroup;
  categories: CategoryWithSubGroups[] = [];
  subcategories: SubCategoryGroups[] = [];
  categoriesSubscription: Subscription;
  initialSubcategoryId: number | null;
  iconTrash = faTrash;
  @Output() createPlaceholder = new EventEmitter<void>();

  constructor(private fb: FormBuilder, private categoryService: CategoryService, 
    private productService: ProductService, private toastr: ToastrService) {
    
  }

  ngOnInit(): void {
    this.initializeForm();
    this.getCategories();

  }

  onSubmit() {
    if (!this.productForm.valid) {
      this.productForm.updateValueAndValidity();
    }

    this.productService.updateProduct(this.productForm.value).subscribe({
      next: () => {
        if (this.isEdit) {
          this.toastr.success('Product updated');
        } else {
          this.toastr.success('Product created');
          this.productForm.reset();
          this.createPlaceholder.emit();
        }     
      }
    })
  }

  getCategories() {
    this.categoryService.getCategories();
    this.categoriesSubscription = this.categoryService.categories$.subscribe({
      next: (categories) => {
        if(categories !== null) {
          this.categories = categories;
          this.categoryId?.setValue(this.categoryId.value);
        }
      }
    })
  }

  addColor() {
    const colorControl = this.fb.control('', Validators.required);
    this.colorsArray.push(colorControl);
  }

  deleteColor(index: number) {
    this.colorsArray.removeAt(index);
  }

  addSpecification() {
    const specification = this.fb.group({
      name: ['', Validators.required],
      value: ['', Validators.required]
    })
    this.specifications.push(specification);
  }

  deleteSpecification(index: number) {
    this.specifications.removeAt(index);
  }

  initializeForm() {
    let id = null;
    let name = '';
    let originalPrice = '';
    let salePrice = '';
    let quantity = '';
    let description = '';
    let subCategoryId = 0;
    let categoryId = 0;
    let brandId = '';
    let colors = this.fb.array([], Validators.required);
    let productSpecifications = this.fb.array<FormGroup>([], Validators.required);

    if (this.product) {
      id = this.product.id;
      name = this.product.name;
      originalPrice = this.product.originalPrice?.toString();
      salePrice = this.product.salePrice?.toString();
      quantity = this.product.quantity?.toString();
      description = this.product.description;
      categoryId = this.product.categoryId;
      this.initialSubcategoryId = this.product.subCategoryId;
      brandId = this.product?.brand?.id.toString();
      this.product.colors?.forEach(color => {
        colors.push(this.fb.control(color.value, Validators.required))
      })
      this.product.productSpecifications?.forEach(spec => {
        const specification =  this.fb.group({
          name: [spec.name, Validators.required],
          value: [spec.value, Validators.required]
        })
        productSpecifications.push(specification)
      })
    }


    this.productForm = this.fb.group({
      id: [id, Validators.required],
      name: [name, Validators.required],
      originalPrice: [originalPrice, [Validators.required, Validators.pattern(/^\d+(\.\d{1,2})?$/)]],
      salePrice: [salePrice, Validators.pattern(/^\d+(\.\d{1,2})?$/)],
      quantity: [quantity, [Validators.required, Validators.pattern(/^(?:0|[1-9]\d*)$/)]],
      description: [description, Validators.required],
      subCategoryId: [{value: subCategoryId, disabled: true}, Validators.required],
      categoryId: [categoryId, Validators.required],
      brandId: [brandId, Validators.required],
      colors: colors,
      productSpecifications: productSpecifications,
    })

    this.categoryId?.valueChanges.pipe(distinctUntilChanged()).subscribe((res: number) => {
      this.subCategoryId?.setValue(this.initialSubcategoryId)
      this.initialSubcategoryId = null;
      if(res) {
        this.subcategories = this.categories.find((category) => category.id === res)!.subcategoryGroups;
        this.subCategoryId?.enable();
      } else {
        this.subCategoryId?.disable();
      }
    })
  }

  ngOnDestroy(): void {
    if (this.categoriesSubscription) {
      this.categoriesSubscription.unsubscribe();
    }
  }

  get name() {
    return this.productForm.get('name');
  }
  get originalPrice() {
    return this.productForm.get('originalPrice');
  }
  get salePrice() {
    return this.productForm.get('salePrice');
  }
  get quantity() {
    return this.productForm.get('quantity');
  }
  get description() {
    return this.productForm.get('description');
  }
  get subCategoryId() {
    return this.productForm.get('subCategoryId');
  }
  get categoryId() {
    return this.productForm.get('categoryId');
  }
  get brandId() {
    return this.productForm.get('brandId');
  }
  get colorsArray() {
    return this.productForm.get('colors') as FormArray;
  }
  get specifications() {
    return this.productForm.get('productSpecifications') as FormArray;
  }
  
}
