import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Reservation } from '../models/reservation.model';

@Injectable({
    providedIn: 'root'
})
export class ReservationService {

    private apiUrl = '/api/reservation';

    constructor(private http: HttpClient) { }

    createReservation(data: { propertyId: number; checkInDate: string; checkOutDate: string }): Observable<any> {
        return this.http.post(this.apiUrl, data);
    }

    getMyReservations(): Observable<Reservation[]> {
        return this.http.get<Reservation[]>(`${this.apiUrl}/my-reservations`);
    }

    getOwnerReservations(): Observable<Reservation[]> {
        return this.http.get<Reservation[]>(`${this.apiUrl}/owner-reservations`);
    }

    confirmReservation(id: number): Observable<any> {
        return this.http.put(`${this.apiUrl}/confirm/${id}`, {});
    }

    rejectReservation(id: number): Observable<any> {
        return this.http.put(`${this.apiUrl}/reject/${id}`, {});
    }

    cancelReservation(id: number): Observable<any> {
        
        return this.http.delete(`${this.apiUrl}/${id}`, { responseType: 'text' as 'json' });
    }

    getBookedDates(propertyId: number): Observable<string[]> {
        return this.http.get<string[]>(`${this.apiUrl}/property/${propertyId}/booked-dates`);
    }
}
