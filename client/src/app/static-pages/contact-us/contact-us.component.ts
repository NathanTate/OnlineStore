import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { faLocationDot, faPhone} from '@fortawesome/free-solid-svg-icons';
import {faClock, faEnvelope} from '@fortawesome/free-regular-svg-icons'
import { ToastrService } from 'ngx-toastr';
import { ReviewService } from '../../reviews/reviews.service';

@Component({
  selector: 'app-contact-us',
  templateUrl: './contact-us.component.html',
  styleUrl: './contact-us.component.css'
})
export class ContactUsComponent implements OnInit{
  feedbackForm: FormGroup;
  iconLocation = faLocationDot;
  iconPhone = faPhone;
  iconClock = faClock;
  iconEmail = faEnvelope;

  constructor(private reviewService: ReviewService, private fb: FormBuilder,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  onSubmit() {
    if(this.feedbackForm.valid) {
      this.reviewService.leaveFeedback(this.feedbackForm.value).subscribe({
        next: () => {
          this.toastr.success('Thanks for the feedback')
        }
      });
    }
  }

  initializeForm() {
    this.feedbackForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['+', [Validators.required, 
        Validators.pattern(/^\+(?:9[976]\d|8[987530]\d|6[987]\d|5[90]\d|42\d|3[875]\d|2[98654321]\d|9[8543210]|8[6421]|6[6543210]|5[87654321]|4[987654310]|3[9643210]|2[70]|7|1)\d{1,14}$/), 
        Validators.maxLength(13)]],
      message: ['', [Validators.required, Validators.maxLength(500)]]
    })
  }

  get name() {
    return this.feedbackForm.get('name');
  }
  
  get email() {
    return this.feedbackForm.get('email');
  }

  get phone() {
    return this.feedbackForm.get('phone');
  }

  get message() {
    return this.feedbackForm.get('message');
  }
}
