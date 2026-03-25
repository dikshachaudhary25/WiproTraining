import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { ToastService } from '../services/toast';
import { AuthService } from '../services/auth';
import { Router } from '@angular/router';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const toastService = inject(ToastService);
  const authService = inject(AuthService);
  const router = inject(Router);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      let errorMsg = 'An unknown error occurred!';
      
      if (error.error instanceof ErrorEvent) {
        
        errorMsg = `Error: ${error.error.message}`;
      } else {
        
        if (error.status === 0) {
          errorMsg = 'Cannot connect to server. Please check your internet connection.';
        } else if (error.status === 401) {
          errorMsg = 'Session expired or unauthorized. Please login.';
          authService.logout();
          router.navigate(['/login']);
        } else if (error.status === 403) {
          errorMsg = 'You are not allowed to perform this action.';
        } else if (error.error && typeof error.error === 'string') {
          errorMsg = error.error;
        } else if (error.error && error.error.message) {
          errorMsg = error.error.message;
        } else {
          errorMsg = `Server returned code: ${error.status}`;
        }
      }

      
      toastService.showError(errorMsg);

      return throwError(() => new Error(errorMsg));
    })
  );
};
