import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './header.html',
  styleUrl: './header.css'
})
export class Header {
  constructor(public auth: AuthService, private router: Router) { }

  logout() {
    this.auth.logout();
    this.router.navigate(['/login']);
  }
}
