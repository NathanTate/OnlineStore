import { NgModule } from "@angular/core";
import { ForgotPasswordComponent } from "./forgot-password.component";
import { CommonModule } from "@angular/common";
import { ReactiveFormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";

@NgModule({
  declarations: [
    ForgotPasswordComponent
  ], 
  imports: [
    CommonModule, 
    ReactiveFormsModule,
    RouterModule.forChild([
      {path: '', component: ForgotPasswordComponent}
    ])
  ]
})
export class ForgotPasswordModule {

}