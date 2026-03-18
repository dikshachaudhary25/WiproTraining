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

    cancelReservation(id: number): Observable<any> {
        // Backend returns Ok("Reservation cancelled") which is plain text, so responseType is needed
        return this.http.delete(`${this.apiUrl}/${id}`, { responseType: 'text' as 'json' });
    }
}
