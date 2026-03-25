import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PLATFORM_ID } from '@angular/core';
import { MyReservationsComponent } from './my-reservations';
import { ReservationService } from '../../services/reservation';
import { of } from 'rxjs';
import { vi } from 'vitest';
import { provideRouter } from '@angular/router';

describe('MyReservationsComponent', () => {
  let component: MyReservationsComponent;
  let fixture: ComponentFixture<MyReservationsComponent>;
  let reservationService: ReservationService;

  beforeEach(async () => {
    const reservationServiceMock = {
      getMyReservations: vi.fn().mockReturnValue(of([])),
      cancelReservation: vi.fn().mockReturnValue(of('Cancelled'))
    };

    await TestBed.configureTestingModule({
      imports: [MyReservationsComponent],
      providers: [
        { provide: ReservationService, useValue: reservationServiceMock },
        { provide: PLATFORM_ID, useValue: 'browser' },
        provideRouter([])
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(MyReservationsComponent);
    component = fixture.componentInstance;
    reservationService = TestBed.inject(ReservationService);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load reservations on init', () => {
    const mockReservations = [{ reservationId: 1, createdAt: new Date().toISOString(), reservationStatus: 'Pending' }];
    vi.mocked(reservationService.getMyReservations).mockReturnValue(of(mockReservations as any));

    component.ngOnInit();

    expect(component.reservations.length).toBe(1);
    expect(component.isLoading).toBe(false);
  });

  it('should call cancelReservation when confirmed', () => {
    vi.spyOn(window, 'confirm').mockReturnValue(true);
    const cancelSpy = vi.mocked(reservationService.cancelReservation);
    
    component.cancelReservation(1);
    
    expect(cancelSpy).toHaveBeenCalledWith(1);
  });
});
