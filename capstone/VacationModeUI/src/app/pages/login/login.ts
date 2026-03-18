import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, RouterLink, CommonModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class LoginComponent {

  email = '';
  password = '';
  errorMessage = '';
  isLoading = false;

  constructor(private auth: AuthService, private router: Router) {}

  login() {

    this.errorMessage = '';

    if (!this.email || !this.password) {
      this.errorMessage = 'Please enter your email and password.';
      return;
    }

    this.isLoading = true;

    this.auth.login({
      email: this.email,
      password: this.password
    }).subscribe({

      next: (token: string) => {

        this.isLoading = false;

        this.auth.saveToken(token);

        this.router.navigate(['/properties']);
      },

      error: () => {

        this.isLoading = false;

        this.errorMessage = 'Invalid email or password.';
      }

    });
  }
}