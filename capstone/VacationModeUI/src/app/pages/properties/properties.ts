import { Component, OnInit, afterNextRender, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { PropertyService } from '../../services/property';
import { ReservationService } from '../../services/reservation';
import { AuthService } from '../../services/auth';
import { Property } from '../../models/property.model';

@Component({
  selector: 'app-properties',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './properties.html',
  styleUrls: ['./properties.css']
})
export class PropertiesComponent implements OnInit {

  properties: Property[] = [];
  filteredProperties: Property[] = [];
  isLoading = true;
  deleteError = '';
  placeholderImage = 'data:image/svg+xml;utf8,<svg xmlns="http://www.w3.org/2000/svg" width="800" height="500"><rect width="100%" height="100%" fill="%23e9eefb"/><text x="50%" y="50%" dominant-baseline="middle" text-anchor="middle" fill="%236a7693" font-family="Arial" font-size="28">VacationMode</text></svg>';

  searchCity = '';
  searchType = '';

  // Reservation state
  reservingPropertyId: number | null = null;
  checkInDate = '';
  checkOutDate = '';
  reservationSuccess = '';
  reservationError = '';
  readonly today = new Date().toISOString().split('T')[0];

  constructor(
    private propertyService: PropertyService,
    private reservationService: ReservationService,
    public auth: AuthService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {
    // Load properties only after the component renders in the browser
    afterNextRender(() => {
      this.loadProperties();
    });
  }

  ngOnInit() {
    // intentionally empty — data loading is deferred to afterNextRender
  }

  loadProperties() {
    this.isLoading = true;
    this.deleteError = '';

    this.propertyService.getAllProperties().subscribe({
      next: (data: Property[]) => {
        this.properties = data;
        this.filteredProperties = data;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  filterProperties() {

    const city = this.searchCity.toLowerCase();
    const type = this.searchType.toLowerCase();

    this.filteredProperties = this.properties.filter(p => {

      const cityMatch =
        !city || (p.city && p.city.toLowerCase().includes(city));

      const typeMatch =
        !type || (p.propertyType && p.propertyType.toLowerCase().includes(type));

      return cityMatch && typeMatch;
    });
  }

  viewProperty(id: number | undefined) {
    if (id !== undefined) {
      this.router.navigate(['/properties', id]);
    }
  }

  editProperty(id: number | undefined) {
    if (id !== undefined) {
      this.router.navigate(['/edit-property', id]);
    }
  }

  deleteProperty(id: number | undefined) {

    if (id === undefined) return;

    if (!confirm('Are you sure you want to delete this property?')) return;

    this.propertyService.deleteProperty(id).subscribe({
      next: () => this.loadProperties(),
      error: () => {
        this.deleteError = 'Failed to delete property. Please try again.';
      }
    });
  }

  /** Check if the current user owns a specific property */
  isPropertyOwner(property: Property): boolean {
    return this.auth.isLoggedIn()
      && this.auth.isOwner()
      && property.ownerId === this.auth.getUserId();
  }

  /** Toggle inline reservation form for a property */
  toggleReserveForm(propertyId: number | undefined) {
    if (propertyId === undefined) return;

    if (this.reservingPropertyId === propertyId) {
      this.reservingPropertyId = null;
      this.resetReservationForm();
      return;
    }

    this.reservingPropertyId = propertyId;
    this.resetReservationForm();
  }

  /** Submit inline reservation */
  submitReservation(propertyId: number | undefined) {
    if (propertyId === undefined) return;

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
      propertyId,
      checkInDate: this.checkInDate,
      checkOutDate: this.checkOutDate
    }).subscribe({
      next: () => {
        this.reservationSuccess = 'Reservation created! Status: Pending.';
        this.reservationError = '';
        this.checkInDate = '';
        this.checkOutDate = '';
      },
      error: (err) => {
        console.error('Reservation failed:', err);
        this.reservationError = err?.error || 'Reservation failed. Please try again.';
        this.reservationSuccess = '';
      }
    });
  }

  private resetReservationForm() {
    this.checkInDate = '';
    this.checkOutDate = '';
    this.reservationSuccess = '';
    this.reservationError = '';
  }
}