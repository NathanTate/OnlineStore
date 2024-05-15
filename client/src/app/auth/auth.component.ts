import { Component, OnDestroy, OnInit } from '@angular/core';
import { Validators } from '@angular/forms';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AuthService } from '../_services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrl: './auth.component.css'
})
export class AuthComponent implements OnInit, OnDestroy{
  loginMode = true;
  submitBtnText = 'Sign In'
  authForm: FormGroup;
  lowercaseValid: boolean;
  uppercaseValid: boolean;
  digitValid: boolean;
  lengthValid: boolean;
  noWhitespace: boolean;
  timeoutId: ReturnType<typeof setTimeout>;

  constructor(private fb: FormBuilder, private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  ngOnDestroy(): void {
    if(this.timeoutId) {
      clearTimeout(this.timeoutId)
    }
  }

  switchMode() {
    this.loginMode = !this.loginMode;
    this.loginMode == true ? this.submitBtnText = 'Sign In'
      : this.submitBtnText = 'Sign Up'
  }

  initializeForm() {
    this.authForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, 
        Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?!.*\s).{6,32}$/)]]
    })
  }

  onSubmit() {
    if(this.loginMode) {
      this.authService.login(this.authForm.value).subscribe({
        next: () => {
          this.router.navigate(['./']);
          this.authForm.reset();
        }
      })
    } else {
      this.authService.register(this.authForm.value).subscribe({
        next: () => {
          this.switchMode(),
          this.authForm.reset();
        }
      })
    }
  }

  checkPassword() {
    if(this.timeoutId) {
      clearTimeout(this.timeoutId)
    }

    this.timeoutId = setTimeout(() => {
      const password = this.authForm.get('password')?.value;
      this.lowercaseValid = /(?=.*[a-z])/.test(password);
      this.uppercaseValid = /(?=.*[A-Z])/.test(password);
      this.digitValid = /(?=.*\d)/.test(password);
      this.lengthValid = password.length >= 6 && password.length <= 32;
      this.noWhitespace = !/\s/.test(password);
    }, 300)
  }
}

// export const length: ValidatorFn = 
//   (control: AbstractControl): ValidationErrors | null => {
//     return control.value.length < 6 || control.value.length > 32 
//       ? {'length': control.value} : null;
// }

// export function length(): ValidatorFn {
//   return (control: AbstractControl): {[key: string]: string} | null =>
//     control.value.length < 6 || control.value.length > 32 
//   ? {'lenght': control.value} : null;
// }

