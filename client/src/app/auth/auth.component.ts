import { Component, OnDestroy, OnInit } from '@angular/core';
import { Validators } from '@angular/forms';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AuthService } from '../_services/auth.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { faCircleCheck } from '@fortawesome/free-regular-svg-icons';
import { CredentialResponse, PromptMomentNotification } from 'google-one-tap';
import { environment } from '../../environments/environment.development';

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
  showVerifyEmail = false;
  iconCheck = faCircleCheck;

  constructor(private fb: FormBuilder, private authService: AuthService,
    private router: Router, private toastr: ToastrService
  ) { 
    this.authService.currentUser$.subscribe({
      next: (user) => {
        if (!!user) {
          this.router.navigate(['/']);
        }
      }
    })
  }

  ngOnInit(): void {
    this.initializeForm();
    this.initializeGoogleAuth();
  }

  ngOnDestroy(): void {
    if(this.timeoutId) {
      clearTimeout(this.timeoutId)
    }
  }

  initializeGoogleAuth() {
     // @ts-ignore
    window.onGoogleLibraryLoad = () => {
       // @ts-ignore
      google.accounts.id.initialize({
        client_id: environment.clientIdGoogle,
        callback: this.handleCredentialResponse.bind(this),
        auto_select: false,
        cancel_on_tap_outside: true
      });
       // @ts-ignore
      google.accounts.id.renderButton(
        // @ts-ignore
        document.getElementById('googleBtn'),
          {theme: "filled_blue", size: "large", shape: 'pill', width: "100%"}
      );
       // @ts-ignore
      google.accounts.id.prompt((notification: PromptMomentNotification) => {})
    }
  }

  async handleCredentialResponse(response: CredentialResponse) {
    await this.authService.loginWithGoogle(response.credential).subscribe({
      next: (creds) => {
        this.router.navigate(['./']);
      }
    })
  } 

  switchMode() {
    this.loginMode = !this.loginMode;
    this.loginMode == true ? this.submitBtnText = 'Sign In'
      : this.submitBtnText = 'Sign Up';
    this.showVerifyEmail = false;
  }

  verifyEmail() {
    this.authService.verifyEmail(this.email?.value,this.token?.value).subscribe({
      next: () => {
        this.toastr.success("Email successfully verified")
      }
    })
  }

  sendVerificationToken() {
    this.authService.sendVerificationToken(this.email?.value).subscribe({
      next: () => {
        this.toastr.info('Check your email')
      }
    })
  }

  initializeForm() {
    this.authForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, 
        Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?!.*\s).{6,32}$/)]],
      token: ['']
    })
  }

  onSubmit() {
    if(!this.authForm.valid) {
      this.toastr.error('Submission of invalid form')
      return;
    }
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
          this.showVerifyEmail = true;
          this.password?.reset();
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

  get email() {
    return this.authForm.get('email');
  }

  get token() {
    return this.authForm.get('token')
  }

  get password() {
    return this.authForm.get('password')
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

