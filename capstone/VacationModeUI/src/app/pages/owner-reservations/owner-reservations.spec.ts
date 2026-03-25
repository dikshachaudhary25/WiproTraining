import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PLATFORM_ID } from '@angular/core';
import { OwnerReservationsComponent } from './owner-reservations';
import { ReservationService } from '../../services/reservation';
import { AuthService } from '../../services/auth';
import { Router } from '@angular/router';
import { of } from 'rxjs';
import { vi } from 'vitest';
import { provideRouter } from '@angular/router';

describe('OwnerReservationsComponent', () => {
  let component: OwnerReservationsComponent;
  let fixture: ComponentFixture<OwnerReservationsComponent>;
  let reservationService: ReservationService;
  let authService: AuthService;

  beforeEach(async () => {
    const reservationServiceMock = {
      getOwnerReservations: vi.fn().mockReturnValue(of([])),
      confirmReservation: vi.fn().mockReturnValue(of({})),
      rejectReservation: vi.fn().mockReturnValue(of({}))
    };
    const authServiceMock = {
      isLoggedIn: vi.fn().mockReturnValue(true),
      isOwner: vi.fn().mockReturnValue(true)
    };

    await TestBed.configureTestingModule({
      imports: [OwnerReservationsComponent],
      providers: [
        { provide: ReservationService, useValue: reservationServiceMock },
        { provide: AuthService, useValue: authServiceMock },
        { provide: PLATFORM_ID, useValue: 'browser' },
        provideRouter([])
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(OwnerReservationsComponent);
    component = fixture.componentInstance;
    reservationService = TestBed.inject(ReservationService);
    authService = TestBed.inject(AuthService);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load owner reservations on init', () => {
    const mockReservations = [{ reservationId: 1 }];
    vi.mocked(reservationService.getOwnerReservations).mockReturnValue(of(mockReservations as any));

    component.ngOnInit();

    expect(component.reservations.length).toBe(1);
    expect(component.isLoading).toBe(false);
  });

  it('should confirm reservation', () => {
    const confirmSpy = vi.mocked(reservationService.confirmReservation);
    component.confirmReservation(1);
    expect(confirmSpy).toHaveBeenCalledWith(1);
  });

  it('should reject reservation', () => {
    const rejectSpy = vi.mocked(reservationService.rejectReservation);
    component.rejectReservation(1);
    expect(rejectSpy).toHaveBeenCalledWith(1);
  });

  it('should navigate to home if not owner', () => {
    const router = TestBed.inject(Router);
    const navigateSpy = vi.spyOn(router, 'navigate');
    vi.mocked(authService.isOwner).mockReturnValue(false);
    
    component.ngOnInit();
    
    expect(navigateSpy).toHaveBeenCalledWith(['/']);
  });
});
