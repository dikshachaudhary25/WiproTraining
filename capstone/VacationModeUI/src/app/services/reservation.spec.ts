import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ReservationService } from './reservation';
import { Reservation } from '../models/reservation.model';
import { vi } from 'vitest';

describe('ReservationService', () => {
  let service: ReservationService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ReservationService]
    });
    service = TestBed.inject(ReservationService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should create a reservation', () => {
    const data = { propertyId: 1, checkInDate: '2026-04-01', checkOutDate: '2026-04-05' };
    service.createReservation(data).subscribe(res => {
      expect(res).toEqual({ success: true });
    });

    const req = httpMock.expectOne('/api/reservation');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(data);
    req.flush({ success: true });
  });

  it('should get my reservations', () => {
    service.getMyReservations().subscribe(res => {
      expect(res.length).toBe(0);
    });

    const req = httpMock.expectOne('/api/reservation/my-reservations');
    expect(req.request.method).toBe('GET');
    req.flush([]);
  });

  it('should get owner reservations', () => {
    service.getOwnerReservations().subscribe(res => {
      expect(res.length).toBe(0);
    });

    const req = httpMock.expectOne('/api/reservation/owner-reservations');
    expect(req.request.method).toBe('GET');
    req.flush([]);
  });

  it('should confirm a reservation', () => {
    service.confirmReservation(123).subscribe(res => {
      expect(res).toBeTruthy();
    });

    const req = httpMock.expectOne('/api/reservation/confirm/123');
    expect(req.request.method).toBe('PUT');
    req.flush({});
  });

  it('should reject a reservation', () => {
    service.rejectReservation(123).subscribe(res => {
      expect(res).toBeTruthy();
    });

    const req = httpMock.expectOne('/api/reservation/reject/123');
    expect(req.request.method).toBe('PUT');
    req.flush({});
  });

  it('should cancel a reservation and handle text response', () => {
    service.cancelReservation(123).subscribe(res => {
      expect(res).toBe('Reservation cancelled');
    });

    const req = httpMock.expectOne('/api/reservation/123');
    expect(req.request.method).toBe('DELETE');
    req.flush('Reservation cancelled');
  });

  it('should get booked dates for a property', () => {
    const dates = ['2026-04-01', '2026-04-02'];
    service.getBookedDates(1).subscribe(res => {
      expect(res).toEqual(dates);
    });

    const req = httpMock.expectOne('/api/reservation/property/1/booked-dates');
    expect(req.request.method).toBe('GET');
    req.flush(dates);
  });
});
