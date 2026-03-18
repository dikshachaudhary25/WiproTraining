import { Component, OnInit, afterNextRender, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { PropertyService } from '../../services/property';
import { ReservationService } from '../../services/reservation';
import { AuthService } from '../../services/auth';
import { Property } from '../../models/property.model';

@Component({
  selector: 'app-property-details',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './property-details.html',
  styleUrls: ['./property-details.css']
})
export class PropertyDetailsComponent implements OnInit {

  property: Property | null = null;
  routeId: number | null = null;
  isLoading = true;

  checkInDate = '';
  checkOutDate = '';

  reservationSuccess = '';
  reservationError = '';

  readonly today = new Date().toISOString().split('T')[0];

  readonly placeholderImage =
    'data:image/svg+xml;utf8,<svg xmlns="http://www.w3.org/2000/svg" width="1200" height="700"><rect width="100%25" height="100%25" fill="%23e9eefb"/><text x="50%25" y="50%25" dominant-baseline="middle" text-anchor="middle" fill="%236a7693" font-family="Arial" font-size="36">VacationMode Property</text></svg>';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private propertyService: PropertyService,
    private reservationService: ReservationService,
    public auth: AuthService,
    private cdr: ChangeDetectorRef
  ) {
    // Load property data only after the component renders in the browser
    afterNextRender(() => {
      const idParam = this.route.snapshot.paramMap.get('id');

      if (!idParam) {
        this.isLoading = false;
        this.cdr.detectChanges();
        return;
      }

      const id = Number(idParam);

      if (isNaN(id)) {
        this.isLoading = false;
        this.cdr.detectChanges();
        return;
      }

      this.routeId = id;
      this.loadProperty(id);
    });
  }

  ngOnInit() {
    // intentionally empty — data loading is deferred to afterNextRender
  }

  loadProperty(id: number) {

    this.isLoading = true;

    this.propertyService.getPropertyById(id).subscribe({
      next: (data: Property) => {
        this.property = data;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Failed to load property:', err);
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  reserveProperty() {

    if (!this.property?.propertyId) return;

    this.reservationSuccess = '';
    this.reservationError = '';

    if (!this.auth.isLoggedIn()) {
      this.reservationError = 'Please log in to make a reservation.';
      this.router.navigate(['/login']);
      return;
    }

    if (!this.checkInDate || !this.checkOutDate) {
      this.reservationError = 'Please select both check-in and check-out dates.';
      return;
    }

    if (new Date(this.checkOutDate) <= new Date(this.checkInDate)) {
      this.reservationError = 'Check-out date must be after check-in date.';
      return;
    }

    this.reservationService.createReservation({
      propertyId: this.property.propertyId,
      checkInDate: this.checkInDate,
      checkOutDate: this.checkOutDate
    }).subscribe({
      next: () => {
        this.reservationSuccess = 'Reservation created successfully. Status: Pending.';
        this.reservationError = '';
        this.checkInDate = '';
        this.checkOutDate = '';
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Reservation failed:', err);
        this.reservationError = err?.error || 'Reservation failed. Please try again.';
        this.reservationSuccess = '';
        this.cdr.detectChanges();
      }
    });
  }
}