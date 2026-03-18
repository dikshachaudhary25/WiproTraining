import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, RouterLink, CommonModule],
  templateUrl: './register.html',
  styleUrls: ['./register.css']
})
export class RegisterComponent {

  user = {
    fullName: '',
    email: '',
    password: '',
    phoneNumber: '',
    role: 'Renter'
  };

  errorMessage = '';
  successMessage = '';
  isLoading = false;

  constructor(private auth: AuthService, private router: Router) {}

  register() {
    this.errorMessage = '';
    this.successMessage = '';

    if (!this.user.fullName || !this.user.email || !this.user.password || !this.user.phoneNumber) {
      this.errorMessage = 'Please fill in all required fields.';
      return;
    }

    this.isLoading = true;

    this.auth.register(this.user).subscribe({
      next: () => {
        this.isLoading = false;
        this.successMessage = 'Registration successful! Redirecting to login...';
        setTimeout(() => this.router.navigate(['/login']), 1500);
      },
      error: (err: any) => {
        this.isLoading = false;
        this.errorMessage = err?.error || 'Registration failed. Please try again.';
      }
    });
  }
}
