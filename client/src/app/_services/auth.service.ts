import { Injectable } from "@angular/core";
import { BehaviorSubject, map } from "rxjs";
import { User } from "../_models/User";
import { HttpClient } from "@angular/common/http";
import { AuthModel } from "../_models/Auth";
import { environment } from "../../environments/environment.development";
import { Router } from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  currentUser$ = new BehaviorSubject<User| null>(null);
  tokenExperationTimer: ReturnType<typeof setTimeout>;
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient, private router: Router) {
   
  }

  login(model: AuthModel) {
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map((user: User) => {
            this.setCurrentUser(user);
      })
    );
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
    this.currentUser$.next(null);
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
    const experationTime = Date.now() - expires.getDate();
    const roles = decodedToken.role;
    user.tokenExperationDate = expires;
    user.roles = roles;
    const isValidToken = user.tokenExperationDate && new Date() < user.tokenExperationDate;
    if(isValidToken) {
      this.currentUser$.next(user);
      localStorage.setItem('user', JSON.stringify(user));
  
      this.autoLogout(experationTime);
    }
  }

  getDecodedToken(token: string | null) {
    if(token) {
      return JSON.parse(atob(token.split('.')[1]))
    }
  }
}