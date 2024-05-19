import { Component, OnInit, inject } from '@angular/core';
import { LoadingSpinnerService } from '../../_services/loading-spinner.service';

@Component({
  selector: 'app-loading-spinner',
  templateUrl: './loading-spinner.component.html',
  styleUrl: './loading-spinner.component.css'
})
export class LoadingSpinnerComponent implements OnInit{
  spinnerService = inject(LoadingSpinnerService);
  isLoading = false;

  ngOnInit(): void {
    this.spinnerService.spinner$.subscribe({
      next: (value) => {
        this.isLoading = value;
      }
    })
  }
}
