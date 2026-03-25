import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastService, ToastMessage } from '../../services/toast';

@Component({
  selector: 'app-toast',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './toast.html',
  styleUrls: ['./toast.css']
})
export class ToastComponent implements OnInit {
  toasts: ToastMessage[] = [];

  constructor(private toastService: ToastService) {}

  ngOnInit() {
    this.toastService.toastState.subscribe(toast => {
      this.toasts.push(toast);
      setTimeout(() => this.remove(toast), 5000);
    });
  }

  remove(toast: ToastMessage) {
    this.toasts = this.toasts.filter(t => t !== toast);
  }
}
