import { Component, EventEmitter, OnInit, Output } from "@angular/core";
import { ModalService } from "../../_services/modal.service";
import { faFilter, faStar } from "@fortawesome/free-solid-svg-icons";
import { FormBuilder, FormGroup } from "@angular/forms";
import { ReviewFilterParams } from "../reviews-filter";

@Component({
  selector: 'app-reviews-filter',
  templateUrl: './reviews-filter.componenet.html',
  styleUrl: './reviews-filter.componenet.css'
})
export class ReviewsFilterComponent implements OnInit{
  readonly modalName: string = 'reviewFilter';
  @Output() filtersChange = new EventEmitter<ReviewFilterParams>();
  params: ReviewFilterParams = {sortColumn: 'date', sortBy: 'desc', stars: 0}
  filterForm: FormGroup;
  iconFilters = faFilter;
  iconStar = faStar;
  
  constructor(private modalService: ModalService, private fb: FormBuilder) {}

  ngOnInit(): void {
    this.filterForm = this.fb.group({
      stars: [0]
    })
  }

  onSubmit() {
    this.params.stars = this.filterForm.get('stars')!.value;
    this.closeModal();
    this.onFiltersChange();
  }

  removeStarsFilter() {
    this.params.stars = 0;
    this.filterForm.reset({
      stars: 0
    })
    this.onFiltersChange();
  }

  onFiltersChange() {
    this.filtersChange.emit(this.params);
  }

  openModal() {
    this.modalService.openModal(this.modalName);
  }

  closeModal() {
    this.modalService.closeModal(this.modalName);
  }
}