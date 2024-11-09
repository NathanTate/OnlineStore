import { Component, OnInit } from '@angular/core';
import { AuthService } from './_services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit{
  title = 'client';

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    if(localStorage.getItem('user')) {
      this.authService.refreshToken().subscribe();
    }
  }
}
