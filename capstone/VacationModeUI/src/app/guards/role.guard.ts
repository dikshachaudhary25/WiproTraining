import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot } from '@angular/router';
import { AuthService } from '../services/auth';
import { ToastService } from '../services/toast';

@Injectable({
  providedIn: 'root'
})
export class RoleGuard implements CanActivate {

  constructor(private auth: AuthService, private router: Router, private toast: ToastService) {}

  canActivate(route: ActivatedRouteSnapshot): boolean {
    const expectedRole = route.data['expectedRole'];

    if (!this.auth.isLoggedIn() || this.auth.getUserRole() !== expectedRole) {
      this.toast.showError('Unauthorized: You do not have permission to view this page.');
      this.router.navigate(['/']);
      return false;
    }
    
    return true;
  }
}
