import { CurrencyPipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { FileHandle } from '../../../_models/FileHandle';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { ProductService } from '../../../_services/product.service';
import { ToastrService } from 'ngx-toastr';
import { ProductRequest } from '../../../_models/Product';

@Component({
  selector: 'app-product-admin',
  templateUrl: './product-admin.component.html',
  styleUrl: './product-admin.component.css'
})
export class ProductAdminComponent implements OnInit{
  productForm: FormGroup;
  trashIcon = faTrash;
  cachedImagesArray: FormArray | null = null;

  constructor(private fb: FormBuilder, private currencyPipe: CurrencyPipe, 
    private sanitizer: DomSanitizer, private productService: ProductService, private toastr: ToastrService) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  onSubmit() {
    if (this.productForm.valid) {
        const formData = new FormData();

        // Append regular form controls
        Object.keys(this.productForm.controls).forEach(key => {
            const control = this.productForm.get(key);
            if (control instanceof FormArray) {
                if (key === 'colors') {
                    // Handle colors array separately
                    (control as FormArray).controls.forEach((colorControl: AbstractControl, index: number) => {
                        if (colorControl instanceof FormControl) {
                            formData.append(`colors[${index}].value`, colorControl.value);
                        }
                    });
                } else {
                    // Handle other FormArrays
                    control.controls.forEach((controlItem, index) => {
                        if (controlItem instanceof FormControl) {
                            formData.append(`${key}[${index}]`, controlItem.value);
                        } else {
                            Object.keys(controlItem.value).forEach(nestedKey => {
                                formData.append(`${key}[${index}].${nestedKey}`, controlItem.value[nestedKey]);
                            });
                        }
                    });
                }
            } else {
                formData.append(key, control?.value);
            }
        });

        // Append files
        const images = this.productForm.get('images') as FormArray;
        images.controls.forEach((fileControl: AbstractControl, index: number) => {
            const fileHandle = fileControl.value as FileHandle;
            if (fileHandle && fileHandle.file) {
                formData.append(`images[${index}]`, fileHandle.file);
            }
        });

        this.productService.addProduct(formData).subscribe({
            next: () => this.toastr.success('Success')
        });
    }
}


  onFileDropped(files: FileList) {
    for(let i = 0; i < files.length; i++) {
      const fileHandle: FileHandle = {
        file: files[i],
        url: this.sanitizer.bypassSecurityTrustUrl(
          window.URL.createObjectURL(files[i])
        )
      }

      const imageControl = this.fb.control(fileHandle, Validators.required);
      this.imagesArray.push(imageControl)
    }
  }

  prepareFormData(productRequest: ProductRequest): FormData {
    const formData = new FormData();

    formData.append('productRequest', new Blob([JSON.stringify(productRequest)], {type: 'multipart/form-data'}))

    return formData
  }

  // transformCurrency(event: Event, control: string) {
  //   const inputControl = this.productForm.get(control);
  //   if(!(typeof inputControl?.value == 'string') || inputControl?.value == '') return;

  //   const numericValue = parseFloat(inputControl?.value.replace(/[^\d.-]/g, ''));
  //   inputControl?.patchValue(numericValue)
    
  //   let formatedAmount = this.currencyPipe.transform(numericValue, '$')
  //   if(!formatedAmount) return;

  //   const inputElement = <HTMLInputElement>event.target;
  //   inputElement.value = formatedAmount;
  // }

  private initializeForm() {
    this.productForm = this.fb.group({
      name: ['', Validators.required],
      originalPrice: ['', [Validators.required, Validators.pattern(/^\d+(\.\d{1,2})?$/)]],
      salePrice: ['', Validators.pattern(/^\d+(\.\d{1,2})?$/)],
      description: ['', Validators.required],
      subCategoryId: ['', Validators.required],
      categoryId: ['', Validators.required],
      brandId: ['', Validators.required],
      colors: this.fb.array([], Validators.required),
      productSpecifications: this.fb.array([], Validators.required),
      images: this.fb.array<FormControl<FileHandle>>([], Validators.required)
    })
  }

  get colorsArray() {
    return this.colors as FormArray;
  }

  addColor() {
    const colorControl = this.fb.control('', Validators.required);
    this.colorsArray.push(colorControl);
  }

  deleteColor(index: number) {
    this.colorsArray.removeAt(index);
  }

  get specifications() {
    return this.productSpecifications as FormArray;
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

  get imagesArray(): FormArray {
    if (!this.cachedImagesArray) {
      this.cachedImagesArray = this.productForm.get('images') as FormArray;
      console.log(this.cachedImagesArray);
    }
    return this.cachedImagesArray;
  }

  deleteImage(index: number) {
    this.imagesArray.removeAt(index);
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
  get colors() {
    return this.productForm.get('colors');
  }
  get productSpecifications() {
    return this.productForm.get('productSpecifications');
  }
  get images() {
    return this.productForm.get('images');
  }

}
