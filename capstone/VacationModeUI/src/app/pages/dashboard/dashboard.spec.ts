import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PLATFORM_ID } from '@angular/core';
import { DashboardComponent } from './dashboard';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { AuthService } from '../../services/auth';
import { vi } from 'vitest';
import { provideRouter } from '@angular/router';

describe('DashboardComponent', () => {
  let component: DashboardComponent;
  let fixture: ComponentFixture<DashboardComponent>;
  let httpMock: HttpTestingController;
  let authService: AuthService;

  beforeEach(async () => {
    const authMock = {
      getToken: vi.fn().mockReturnValue('fake-token')
    };

    await TestBed.configureTestingModule({
      imports: [DashboardComponent, HttpClientTestingModule],
      providers: [
        { provide: AuthService, useValue: authMock },
        { provide: PLATFORM_ID, useValue: 'browser' },
        provideRouter([])
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(DashboardComponent);
    component = fixture.componentInstance;
    httpMock = TestBed.inject(HttpTestingController);
    authService = TestBed.inject(AuthService);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load dashboard stats on init', () => {
    const mockStats = { totalProperties: 5, totalReservations: 10, totalEarnings: 5000, upcomingReservations: [] };
    component.ngOnInit();

    const req = httpMock.expectOne('/api/dashboard/owner');
    expect(req.request.method).toBe('GET');
    expect(req.request.headers.get('Authorization')).toBe('Bearer fake-token');
    req.flush(mockStats);

    expect(component.stats).toEqual(mockStats);
    expect(component.isLoading).toBe(false);
  });

  it('should handle dashboard load error', () => {
    component.loadDashboard();
    const req = httpMock.expectOne('/api/dashboard/owner');
    req.error(new ProgressEvent('error'));

    expect(component.error).toBe('Could not load analytics.');
    expect(component.isLoading).toBe(false);
  });
});
