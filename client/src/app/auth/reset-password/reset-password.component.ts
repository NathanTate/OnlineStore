import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthService } from '../../_services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { length } from '../../checkout/checkout.component';
import { ResetPasswordModel } from '../../_models/Auth';
import { ActivatedRoute, Params } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.css'
})
export class ResetPasswordComponent implements OnInit, OnDestroy {
  resetForm: FormGroup;
  resetPasswordModel: ResetPasswordModel;
  subscription: Subscription;
  
  constructor(private fb: FormBuilder, private authService: AuthService, private toastr: ToastrService, private route: ActivatedRoute) {
    this.resetPasswordModel = {email: '', token: '', passwordDto: {password: '', confirmPassword: ''}}
  }

  ngOnInit(): void {
    this.initializeForm();
    this.subscription = this.route.queryParams.subscribe({
      next: (params: Params) => {
        this.resetPasswordModel.email = params['email'],
        this.resetPasswordModel.token = params['token']
      }
    })
  }

  onSubmit() {
    if (!this.resetForm.valid) return;
    this.resetPasswordModel.passwordDto = this.resetForm.value;
    this.authService.resetPassword(this.resetPasswordModel).subscribe({
      next: () => {
        this.toastr.success('Password reset successfully')
      },
      complete: () => {
        this.resetForm.reset();
      }
    });
  }

  initializeForm() {
    this.resetForm = this.fb.group({
      password: ['', [Validators.required, length(6, 32), 
        Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?!.*\s).{6,32}$/)]],
      confirmPassword: ['', [Validators.required]],
    }, { validator: this.passwordMatchValidator })
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  passwordMatchValidator(formGroup: AbstractControl): ValidationErrors | null {
    const passwordControl = formGroup.get('password');
    const confirmPasswordControl = formGroup.get('confirmPassword');

    if (!passwordControl || !confirmPasswordControl) {
      return null;
    }

    if (passwordControl.value !== confirmPasswordControl.value) {
      confirmPasswordControl.setErrors({ passwordMismatch: true });
      return { passwordMismatch: true };
    } else {
      confirmPasswordControl.setErrors(null);
      return null;
    }
  }

  get password() {
    return this.resetForm.get('password');
  }

  get confirmPassword() {
    return this.resetForm.get('confirmPassword')
  }
}
