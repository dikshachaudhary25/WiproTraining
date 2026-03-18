import { Component, OnInit, afterNextRender, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ReservationService } from '../../services/reservation';
import { PropertyService } from '../../services/property';
import { Reservation } from '../../models/reservation.model';
import { Property } from '../../models/property.model';

@Component({
  selector: 'app-my-reservations',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './my-reservations.html',
  styleUrls: ['./my-reservations.css']
})
export class MyReservationsComponent implements OnInit {

  reservations: Reservation[] = [];
  isLoading = false;
  properties: Property[] = [];

  constructor(
    private reservationService: ReservationService,
    private propertyService: PropertyService,
    private cdr: ChangeDetectorRef
  ) {
    // Load reservations only after the component renders in the browser
    afterNextRender(() => {
      this.isLoading = true;
      this.loadProperties();
    });
  }

  ngOnInit() {
    // intentionally empty — data loading is deferred to afterNextRender
  }

  private loadProperties() {
    this.propertyService.getAllProperties().subscribe({
      next: (properties: Property[]) => {
        this.properties = properties;
        this.loadReservations();
      },
      error: (err) => {
        console.error('Error loading properties', err);
        this.loadReservations();
      }
    });
  }

  private loadReservations() {
    this.reservationService.getMyReservations().subscribe({
      next: (data: Reservation[]) => {
        this.reservations = data.map(r => ({
          ...r,
          property: this.properties.find(p => p.propertyId === r.propertyId)
        }));
        // Sort by checkInDate descending or createdAt
        this.reservations.sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        console.error('Error loading reservations', err);
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  cancelReservation(id: number) {
    if (confirm('Are you sure you want to cancel this reservation?')) {
      this.isLoading = true;
      this.reservationService.cancelReservation(id).subscribe({
        next: () => {
          this.loadReservations();
        },
        error: (err) => {
          console.error('Error cancelling reservation', err);
          this.isLoading = false;
          this.cdr.detectChanges();
        }
      });
    }
  }
}
