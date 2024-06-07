import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable, map, take } from "rxjs";
import { User } from "../_models/User";
import { HttpClient, HttpParams } from "@angular/common/http";
import { AuthModel, ResetPasswordModel } from "../_models/Auth";
import { environment } from "../../environments/environment.development";
import { Router } from "@angular/router";
import { CartService } from "./cart.service";
import { generateHttpParams } from "../shared/httpParamsHelper";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  currentUserSubject = new BehaviorSubject<User| null>(null);
  currentUser$ = this.currentUserSubject.asObservable();
  tokenExperationTimer: ReturnType<typeof setTimeout>;
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient, private router: Router, private cartService: CartService) {
   
  }

  login(model: AuthModel) {
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
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

  autoLogin() {
    let user: User = JSON.parse(localStorage.getItem('user') ?? '{}')
    if(Object.keys(user).length === 0) return;
    this.setCurrentUser(user);
  }

  logout() {
    this.currentUserSubject.next(null);
    this.cartService.cartSubject.next(null);
    this.cartService.itemsInCart = 0;
    this.router.navigate(['/auth'])
    if(this.tokenExperationTimer) {
      clearTimeout(this.tokenExperationTimer)
    }
    localStorage.removeItem('user');
  }

  autoLogout(expiresIn: number) {
    this.tokenExperationTimer = setTimeout(() => this.logout(), expiresIn);
  }

  setCurrentUser(user: User) {
    const decodedToken = this.getDecodedToken(user.token);
    const expires = new Date(decodedToken.exp * 1000)
    const experationTime = expires.getDate() - Date.now();
    const roles = decodedToken.role;
    user.tokenExperationDate = expires;
    user.roles = [];
    Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);
    const isValidToken = user.tokenExperationDate && new Date() < user.tokenExperationDate;
    if(isValidToken) {
      this.currentUserSubject.next(user);
      localStorage.setItem('user', JSON.stringify(user));
      this.cartCreate();
      this.autoLogout(experationTime);
    }
  }

  getDecodedToken(token: string | null) {
    if(token) {
      return JSON.parse(atob(token.split('.')[1]))
    }
  }

  private cartCreate(): void {
    this.cartService.cartExists().subscribe({
      next: (exists: boolean) => {
        if(!exists) {
          this.cartService.createCart().subscribe({
            next: () => this.cartService.getCart().subscribe()
          });
        } else {
          this.cartService.getCart().subscribe();
        }
      }
    })
  }
}