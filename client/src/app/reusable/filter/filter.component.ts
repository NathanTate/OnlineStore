import { Component, EventEmitter, Input, OnDestroy, OnInit, Output, Renderer2 } from '@angular/core';
import { SubCategory } from '../../_models/Categories';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { faChevronDown, faChevronUp } from '@fortawesome/free-solid-svg-icons';
import { Subscription } from 'rxjs';
import { ProductParams } from '../../_models/ProductParams';

@Component({
  selector: 'app-filter',
  templateUrl: './filter.component.html',
  styleUrl: './filter.component.css'
})
export class FilterComponent implements OnInit, OnDestroy{
  @Input() colors: Set<string>;
  @Input() brands: Set<string>;
  @Input() subCategories: SubCategory[];
  @Output() filterParams = new EventEmitter<ProductParams>;
  filterForm: FormGroup;
  chevronDown = faChevronDown;
  chevronUp = faChevronUp;
  selectedFiltersAmount: number = 0;
  subscription: Subscription;

  isVisible: IsVisible & { [key: string]: boolean } = {
    category: true,
    price: true,
    color: true,
    filter: true,
    brand: true,
    filterMenu: false
  };

  priceRanges: {value:string, label:string, checked:boolean}[] = [
    { value: '0-1000', label: '$0.00 - $1,000.00', checked: false },
    { value: '1000-2000', label: '$1,000.00 - $2,000.00', checked: false },
    { value: '2000-3000', label: '$2,000.00 - $3,000.00', checked: false },
    { value: '3000-4000', label: '$3,000.00 - $4,000.00', checked: false },
    { value: '4000-5000', label: '$4,000.00 - $5,000.00', checked: false },
    { value: '5000-6000', label: '$5,000.00 - $6,000.00', checked: false },
    { value: '6000-7000', label: '$6,000.00 - $7,000.00', checked: false },
    { value: '7000-999999', label: '$7,000.00 And Above', checked: false }
  ];

  constructor(private fb: FormBuilder, private render: Renderer2) {}

  ngOnInit(): void {
    this.initializeForm();
    this.subscription = this.filterForm.valueChanges.subscribe(() => {
      this.selectedFiltersAmount = this.getSelectedFiltersAmount();
    })
  }

  onToggle(identifier: string) {
    this.isVisible[identifier] = !this.isVisible[identifier];
    if(this.isVisible['filterMenu']) {
      this.render.addClass(document.body, 'modal-open');
      return;
    }
    this.render.removeClass(document.body, 'modal-open');
  }

  onCheckChange(event: Event, arrayName: string) {
    const formArray = this.filterForm.get(arrayName) as FormArray;
    const element = <HTMLInputElement>event.target;

    if((element).checked) {
      formArray.push(new FormControl(element.value))
    } else {
      formArray.controls.forEach((control: AbstractControl, index: number) => {
        if(control.value == element.value) {
          formArray.removeAt(index)
          return;
        }
      })
    }
  }

  onResize() {
    if(window.innerWidth > 768) {
      this.isVisible.filterMenu = false;
    }
  }

  onSubmit() {
    this.filterParams.emit(this.filterForm.value);
  }

  initializeForm() {
    this.filterForm = this.fb.group({
      categoryId: [2],
      subCategories: this.fb.array([]),
      priceMin: [0],
      priceMax: [0],
      colors: this.fb.array([]),
      brands: this.fb.array([])
    })
  }

  getSelectedFiltersAmount(): number {
    let selectedFilters = 0;

    selectedFilters += this.priceRanges.filter(x => x.checked).length;

    const subCategories = this.filterForm.get('subCategories') as FormArray;
    selectedFilters += subCategories.length;

    const colorsArray = this.filterForm.get('colors') as FormArray;
    selectedFilters += colorsArray.length;

    const brandsArray = this.filterForm.get('brands') as FormArray;
    selectedFilters += brandsArray.length;

    return selectedFilters;
  }

  updatePriceValues(event: Event) {
    const element = <HTMLInputElement>event.target;
    if(element.checked) {
      this.priceRanges.find(x => x.value == element.value)!.checked = true;
    } else {
      this.priceRanges.find(x => x.value == element.value)!.checked = false;
    }

    const checkedRanges = this.priceRanges.filter(range => range.checked)
    if(checkedRanges.length > 0) {
      const min = checkedRanges[0].value.split('-')[0];
      const max = checkedRanges[checkedRanges.length - 1].value.split('-')[1];
      this.filterForm.patchValue({priceMin: +min, priceMax: +max})
    } else {
      this.filterForm.patchValue({priceMin: 0, priceMax: 0})
    }
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

}

interface IsVisible {
  category: boolean,
  price: boolean,
  color: boolean,
  filter: boolean,
  brand: boolean,
  filterMenu: boolean;
}
