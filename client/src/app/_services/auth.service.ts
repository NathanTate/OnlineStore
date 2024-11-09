import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable, map, switchMap, take, tap } from "rxjs";
import { User } from "../_models/User";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { AuthModel, ResetPasswordModel } from "../_models/Auth";
import { environment } from "../../environments/environment.development";
import { Router } from "@angular/router";
import { CartService } from "./cart.service";
import { OrderService } from "./order.service";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  currentUserSubject = new BehaviorSubject<User| null>(null);
  currentUser$ = this.currentUserSubject.asObservable();
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient, private router: Router, 
    private cartService: CartService, private orderService: OrderService) {
   
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem('user');
  }

  login(model: AuthModel) {
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map((user: User) => {
          this.setCurrentUser(user);
      })
    );
  }

  refreshToken() {
    return this.http.post<void>(this.baseUrl + 'account/refreshToken', {}).pipe(
      tap(() => {
        const user = JSON.parse(localStorage.getItem('user') ?? '');
        user ? this.setCurrentUser(user) : this.router.navigate(['/auth']);
      })
    );
  }

  loginWithGoogle(credentials: string) {
    const headers = new HttpHeaders().set('Content-type', 'application/json');
    return this.http.post<User>(this.baseUrl + 'account/external-login', JSON.stringify(credentials), {headers: headers}).pipe(
      map((user: User) => {
        this.setCurrentUser(user);
      })
    );
  }

  verifyEmail(email: string, token: string) {
    return this.http.post<void>(this.baseUrl + 'account/VerifyEmail', {email: email, token: token});
  }

  sendVerificationToken(email: string) {
    return this.http.post<void>(this.baseUrl + 'account/SendEmailToken', {email: email});
  }

  sendResetLink(email: string) {
    let httpParams = new HttpParams().set('email', email)
    return this.http.post<void>(this.baseUrl + 'account/forgotPassword', email, {params: httpParams});
  }

  resetPassword(model: ResetPasswordModel) {
    let httpParams = new HttpParams().set('email', model.email);
    httpParams = httpParams.append('token', model.token);
    return this.http.post<void>(this.baseUrl + 'account/resetPassword', model.passwordDto, {params: httpParams})
  }

  hasRole(roles: string[]): Observable<boolean> {
    return this.currentUser$.pipe(take(1), map(user => {
      if(!user || !user.roles) return false;
      
      return user.roles.some(role => roles.includes(role.toUpperCase()));
    }))
  }

  register(model: AuthModel) { 
    return this.http.post(this.baseUrl + 'account/register', model, {responseType: 'text'});
  }

  logout() {
    localStorage.removeItem('user');
    this.http.post(this.baseUrl + 'account/Logout', {}).subscribe(() => {
      this.currentUserSubject.next(null);
      this.orderService.ordersCache.clear();
      this.cartService.cartSubject.next(null);
      this.cartService.itemsInCart = 0;
      this.router.navigate(['/auth'])
    })
  }


  setCurrentUser(user: User) {
    this.cartCreate();
    this.currentUserSubject.next(user);
    localStorage.setItem('user', JSON.stringify(user));
  }


  private cartCreate(): void {
    this.cartService.cartExists().pipe(
      switchMap((exists: boolean) => {
        return exists ? this.cartService.getCart() : this.cartService.createCart().pipe(
          switchMap(() => this.cartService.getCart())
        )  
      })
    ).subscribe()
  }
}