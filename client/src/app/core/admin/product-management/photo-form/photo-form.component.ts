import { Component, Input, OnInit } from '@angular/core';
import { FileHandle } from '../../../../_models/FileHandle';
import { DomSanitizer } from '@angular/platform-browser';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { faTrash, faUndo } from '@fortawesome/free-solid-svg-icons';
import { Product, ProductImage, SetMainPhotoRequest } from '../../../../_models/Product';
import { ToastrService } from 'ngx-toastr';
import { ProductService } from '../../../../_services/product.service';

@Component({
  selector: 'app-photo-form',
  templateUrl: './photo-form.component.html',
  styleUrl: './photo-form.component.css'
})
export class PhotoFormComponent implements OnInit {
  imageForm: FormGroup;
  iconTrash = faTrash;
  iconUndo = faUndo;
  removedImagesIds: number[] = [];
  @Input({required: true}) product: Product;
  @Input() isEdit: boolean = false;
  constructor(private sanitizer: DomSanitizer, private fb: FormBuilder, 
    private productService: ProductService, private toastr: ToastrService) {
    
  }

  ngOnInit(): void {
    this.initializeForm();
  }

  onSubmit() {
    if (!this.imageForm.valid) {
      this.toastr.error('Submition of invalid form')
    }

    let formData = this.toFormData(this.imageForm);
    this.productService.updatePhotos(formData).subscribe({
      next: () => {
        if (this.isEdit) {
          this.toastr.success('Photos Updated')
          this.product.productImages = this.product.productImages.filter(i => !this.removedImagesIds.includes(i.id))
          this.removedImagesIds = [];
        } else {
          this.toastr.success('Photos Uploaded')
          this.imageForm.reset();
        }
      }
    });
  }

  toFormData(imageForm: FormGroup): FormData {
    let formData: any = new FormData();
    console.log(imageForm.controls)
    for (let key in imageForm.controls) {
      const control = imageForm.get(key);
      if (control instanceof FormArray) {
        if(key === 'images') {
          control.controls.forEach((controlItem, index) => {
            formData.append(`PhotoCollection`, controlItem.value.file);
          })
        } else {
          control.controls.forEach((controlItem, index) => {
            formData.append(`${key}[${index}]`, controlItem.value);
          })
        }
      } else {
        formData.append(key, control?.value);
      }
    }
    return formData;
  }

  initializeForm() {
    this.imageForm = this.fb.group({
      itemId: [this.product.id, Validators.required],
      images: this.fb.array([], this.product.productImages ? Validators.nullValidator : Validators.required),
      idsToRemove: this.fb.array([]),
      mainImageIndex: [0, Validators.required]
    })
  }

  setMainPhoto(id: number) {
    let model: SetMainPhotoRequest = {
      itemId: this.product.id,
      photoId: id
    }
    this.productService.setMainPhoto(model).subscribe({
      next: () => {
        this.product.productImages.forEach(i => i.isMain = false);
        this.product.productImages.find(i => i.id === id)!.isMain = true;
        this.toastr.success('Main image changed')
      }
    })
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

  onFileChange(event: EventTarget | null) {
    if(event && event instanceof HTMLInputElement) {
      this.onFileDropped(event.files!)
    }
  }

  formatBytes(bytes: number, decimals: number = 0) {
    if (bytes === 0) {
      return '0 Bytes';
    }
    const k = 1024;
    const dm = decimals <= 0 ? 0 : decimals || 2;
    const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
  }

  removeImage(id: number) {
    this.idsToRemove.push(this.fb.control(id))
    this.removedImagesIds.push(id)
  }

  undoRemoveImage(id: number) {
    const controlIndex = this.idsToRemove.controls.findIndex(c => c.value == id);
    if (controlIndex !== -1) {
      this.idsToRemove.removeAt(controlIndex);
    }
    const removeAt = this.removedImagesIds.findIndex(x => x === id)
    this.removedImagesIds.splice(removeAt, 1)
  }

  deleteImage(index: number) {
    this.imagesArray.removeAt(index);
  }

  get imagesArray() {
    return this.imageForm.get('images') as FormArray;
  }

  get idsToRemove() {
    return this.imageForm.get('idsToRemove') as FormArray;
  }
}
