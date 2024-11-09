import { NgModule } from "@angular/core";
import { AuthComponent } from "./auth.component";
import { RouterModule, Routes } from "@angular/router";

const routes: Routes = [
  {path: '', component: AuthComponent},
  {path: 'forgot-password', loadChildren: () => import('./forgot-password/forgot-password.module')
    .then(m => m.ForgotPasswordModule)},
  {path: 'reset-password', loadChildren: () => import('./reset-password/reset-password.module')
    .then(m => m.ResetPasswordModule)}
]

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class AuthRoutingModule {

}