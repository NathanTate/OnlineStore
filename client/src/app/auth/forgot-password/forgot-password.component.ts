import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { AuthService } from '../../_services/auth.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.css'
})
export class ForgotPasswordComponent{
  emailControl = new FormControl<string>('', [Validators.required, Validators.email]);
  tokenSent = false;
  
  constructor(private authService: AuthService) {}

  onSubmit() {
    if (!this.emailControl.valid || this.emailControl.value == null) return;
    this.authService.sendResetLink(this.emailControl.value).subscribe({
      next: () => {
        this.tokenSent = true;
      },
      complete: () => {
        this.emailControl.reset();
      }
    });
  }
}
