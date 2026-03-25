import { TestBed } from '@angular/core/testing';
import { Router, ActivatedRouteSnapshot } from '@angular/router';
import { RoleGuard } from './role.guard';
import { AuthService } from '../services/auth';
import { ToastService } from '../services/toast';
import { vi } from 'vitest';

describe('RoleGuard', () => {
  let guard: RoleGuard;
  let authService: AuthService;
  let router: Router;
  let toastService: ToastService;

  beforeEach(() => {
    const authMock = {
      isLoggedIn: vi.fn(),
      getUserRole: vi.fn()
    };
    const routerMock = {
      navigate: vi.fn()
    };
    const toastMock = {
      showError: vi.fn()
    };

    TestBed.configureTestingModule({
      providers: [
        RoleGuard,
        { provide: AuthService, useValue: authMock },
        { provide: Router, useValue: routerMock },
        { provide: ToastService, useValue: toastMock }
      ]
    });
    guard = TestBed.inject(RoleGuard);
    authService = TestBed.inject(AuthService);
    router = TestBed.inject(Router);
    toastService = TestBed.inject(ToastService);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });

  it('should return true if user is logged in and has expected role', () => {
    vi.mocked(authService.isLoggedIn).mockReturnValue(true);
    vi.mocked(authService.getUserRole).mockReturnValue('Owner');
    
    const route = { data: { expectedRole: 'Owner' } } as any as ActivatedRouteSnapshot;
    expect(guard.canActivate(route)).toBe(true);
  });

  it('should return false and navigate if user is not logged in', () => {
    vi.mocked(authService.isLoggedIn).mockReturnValue(false);
    
    const route = { data: { expectedRole: 'Owner' } } as any as ActivatedRouteSnapshot;
    expect(guard.canActivate(route)).toBe(false);
    expect(toastService.showError).toHaveBeenCalled();
    expect(router.navigate).toHaveBeenCalledWith(['/']);
  });

  it('should return false if user role does not match expected role', () => {
    vi.mocked(authService.isLoggedIn).mockReturnValue(true);
    vi.mocked(authService.getUserRole).mockReturnValue('Renter');
    
    const route = { data: { expectedRole: 'Owner' } } as any as ActivatedRouteSnapshot;
    expect(guard.canActivate(route)).toBe(false);
    expect(toastService.showError).toHaveBeenCalledWith('Unauthorized: You do not have permission to view this page.');
  });
});
