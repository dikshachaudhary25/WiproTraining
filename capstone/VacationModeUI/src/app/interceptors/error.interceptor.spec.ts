import { TestBed } from '@angular/core/testing';
import { provideHttpClientTesting, HttpTestingController } from '@angular/common/http/testing';
import { HttpClient, HttpErrorResponse, provideHttpClient, withInterceptors } from '@angular/common/http';
import { errorInterceptor } from './error.interceptor';
import { AuthService } from '../services/auth';
import { ToastService } from '../services/toast';
import { Router } from '@angular/router';
import { vi } from 'vitest';

describe('ErrorInterceptor', () => {
  let httpMock: HttpTestingController;
  let httpClient: HttpClient;
  let authService: AuthService;
  let toastService: ToastService;
  let router: Router;

  beforeEach(() => {
    const authMock = { logout: vi.fn() };
    const toastMock = { showError: vi.fn() };
    const routerMock = { navigate: vi.fn() };

    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(withInterceptors([errorInterceptor])),
        provideHttpClientTesting(),
        { provide: AuthService, useValue: authMock },
        { provide: ToastService, useValue: toastMock },
        { provide: Router, useValue: routerMock }
      ]
    });

    httpClient = TestBed.inject(HttpClient);
    httpMock = TestBed.inject(HttpTestingController);
    authService = TestBed.inject(AuthService);
    toastService = TestBed.inject(ToastService);
    router = TestBed.inject(Router);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should logout and navigate to login on 401 status', () => {
    httpClient.get('/api/test').subscribe({
      error: (err) => {
        expect(err.message).toBe('Session expired or unauthorized. Please login.');
      }
    });

    const req = httpMock.expectOne('/api/test');
    req.flush('Unauthorized', { status: 401, statusText: 'Unauthorized' });

    expect(authService.logout).toHaveBeenCalled();
    expect(router.navigate).toHaveBeenCalledWith(['/login']);
    expect(toastService.showError).toHaveBeenCalledWith('Session expired or unauthorized. Please login.');
  });

  it('should show toast error on 403 status', () => {
    httpClient.get('/api/test').subscribe({
       error: (err) => {
         expect(err.message).toBe('You are not allowed to perform this action.');
       }
    });

    const req = httpMock.expectOne('/api/test');
    req.flush('Forbidden', { status: 403, statusText: 'Forbidden' });

    expect(toastService.showError).toHaveBeenCalledWith('You are not allowed to perform this action.');
  });

  it('should show custom error message if provided by backend', () => {
    const customMsg = 'Custom error from server';
    httpClient.get('/api/test').subscribe({
        error: (err) => {
            expect(err.message).toBe(customMsg);
        }
    });

    const req = httpMock.expectOne('/api/test');
    req.flush(customMsg, { status: 400, statusText: 'Bad Request' });

    expect(toastService.showError).toHaveBeenCalledWith(customMsg);
  });
});
