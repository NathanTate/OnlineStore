import { NgModule } from "@angular/core";
import { SharedModule } from "../shared/shared.module";
import { AuthComponent } from "./auth.component";
import { AuthRoutingModule } from "./auth-routing.module";

@NgModule({
  declarations: [
    AuthComponent,
  ],
  imports: [  
    SharedModule,
    AuthRoutingModule
  ],
  exports: [

  ]
})
export class AuthModule {

}