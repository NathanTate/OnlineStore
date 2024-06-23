import { Component, EventEmitter, OnInit, Output } from "@angular/core";
import { ModalService } from "../../_services/modal.service";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ReviewRequest } from "../../_models/Review";
import { Subscription } from "rxjs";

@Component({
  selector: 'app-review-form',
  templateUrl: './review-form.componenet.html',
  styleUrl: './review-form.componenet.css'
})
export class ReviewFormComponent implements OnInit{
  reviewForm: FormGroup;
  subscription: Subscription;
  isSubmitted: boolean = false;
  @Output() handleSubmit = new EventEmitter<ReviewRequest>();

  constructor(private modalService: ModalService, private fb: FormBuilder) {}

  ngOnInit(): void {
    this.initializeForm();
    this.subscription = this.modalService.getModalState('review').subscribe({
      next: (isOpen) => {
        if(isOpen === false) {
          this.reviewForm.reset()
          this.isSubmitted = false;
        };
      }
    })
  }

  onSubmit() {
    this.isSubmitted = true;
    if (!this.reviewForm.valid) return;
    this.handleSubmit.emit(this.reviewForm.value);
    this.modalService.closeModal('review');
  }

  onRatingSelected(rating: number) {
    this.ratingScore?.setValue(rating);
    this.ratingScore?.markAsTouched();
    this.ratingScore?.updateValueAndValidity();
  }

  initializeForm() {
    this.reviewForm = this.fb.group({
      pros: ['', Validators.required],
      cons: ['', Validators.required],
      comment: ['', Validators.maxLength(500)],
      productId: [''],
      ratingScore: ['', [Validators.required]],
      orderStatus: ['']
    })
  }

  openModal() {
    this.modalService.openModal('review');
  }

  get pros() {
    return this.reviewForm.get('pros');
  }

  get cons() {
    return this.reviewForm.get('cons');
  }

  get comment() {
    return this.reviewForm.get('comment');
  }

  get ratingScore() {
    return this.reviewForm.get('ratingScore');
  }

  get orderStatus() {
    return this.reviewForm.get('orderStatus');
  }

}