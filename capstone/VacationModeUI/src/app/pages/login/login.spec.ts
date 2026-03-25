import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { of, throwError } from 'rxjs';
import { vi } from 'vitest';
import { AuthService } from '../../services/auth';

import { LoginComponent } from './login';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LoginComponent, HttpClientTestingModule],
      providers: [provideRouter([{ path: 'properties', component: LoginComponent }])],
    }).compileComponents();

    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should immediately block login and raise an error message if form is missing inputs', () => {
    component.email = '';
    component.password = '';
    component.login();
    expect(component.errorMessage).toBe('Please enter your email and password.');
    expect(component.isLoading).toBe(false);
  });

  it('should cleanly execute authService login if form is dynamically validated', () => {
    const authService = TestBed.inject(AuthService);
    vi.spyOn(authService, 'login').mockReturnValue(of('mock-jwt-token'));
    vi.spyOn(authService, 'saveToken').mockImplementation(() => {});

    component.email = 'diksha@gmail.com';
    component.password = 'Diksha';
    component.login();
    
    expect(authService.login).toHaveBeenCalledWith({ email: 'diksha@gmail.com', password: 'Diksha' });
    expect(authService.saveToken).toHaveBeenCalledWith('mock-jwt-token');
    expect(component.isLoading).toBe(false);
  });
});
