import { TestBed } from '@angular/core/testing';
import { vi } from 'vitest';

import { AuthService } from './auth';
import { provideHttpClient } from '@angular/common/http';

describe('AuthService', () => {
  let service: AuthService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideHttpClient()]
    });
    service = TestBed.inject(AuthService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should return false when isLoggedIn has no token', () => {
    vi.spyOn(Storage.prototype, 'getItem').mockReturnValue(null);
    expect(service.isLoggedIn()).toBe(false);
  });

  it('should return true when isLoggedIn has a valid token', () => {
    vi.spyOn(Storage.prototype, 'getItem').mockReturnValue('fake-jwt-token');
    expect(service.isLoggedIn()).toBe(true);
  });

  it('should securely wipe localStorage traces completely on logout', () => {
    const removeItemSpy = vi.spyOn(Storage.prototype, 'removeItem');
    service.logout();
    expect(removeItemSpy).toHaveBeenCalledWith('token');
    expect(removeItemSpy).toHaveBeenCalledWith('userRole');
    expect(removeItemSpy).toHaveBeenCalledWith('userId');
  });
});
