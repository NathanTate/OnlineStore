import { Directive, Input, OnDestroy, OnInit, TemplateRef, ViewContainerRef } from "@angular/core";
import { AuthService } from "../_services/auth.service";
import { User } from "../_models/User";
import { Subscription, take } from "rxjs";

@Directive({
  selector: '[appHasRole]'
})
export class HasRoleDirective implements OnInit, OnDestroy {
  @Input() appHasRole: string[] = [];
  user: User | null;
  subscription: Subscription;

  constructor(private vcRef: ViewContainerRef, private templateRef: TemplateRef<any>,
    private authService: AuthService) {
     
  }

  ngOnInit(): void {
    this.subscription = this.authService.currentUser$.subscribe({
      next: (user) => {
        this.user = user;
        this.hasRole();
      }
    }) 
  }

  hasRole() {
    if(this.user && this.user.roles.some(role => this.appHasRole.includes(role))) {
      this.vcRef.createEmbeddedView(this.templateRef);
    } else {
      this.vcRef.clear();
    }
  }
  ngOnDestroy(): void {
    if(this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}