import { Component, OnInit, ChangeDetectorRef, Inject, PLATFORM_ID } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ReservationService } from '../../services/reservation';
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
  isCancelling: Record<number, boolean> = {};
  properties: Property[] = [];

  constructor(
    private reservationService: ReservationService,
    private cdr: ChangeDetectorRef,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}

  ngOnInit() {
    if (isPlatformBrowser(this.platformId)) {
      this.isLoading = true;
      this.loadReservations();
    }
  }

  private loadReservations() {
    this.reservationService.getMyReservations().subscribe({
      next: (data: Reservation[]) => {
        this.reservations = data;
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
    if (this.isCancelling[id]) return;
    if (confirm('Are you sure you want to cancel this reservation?')) {
      this.isCancelling[id] = true;
      this.reservationService.cancelReservation(id).subscribe({
        next: () => {
          this.isCancelling[id] = false;
          this.loadReservations();
        },
        error: (err) => {
          console.error('Error cancelling reservation', err);
          this.isCancelling[id] = false;
          this.cdr.detectChanges();
        }
      });
    }
  }
}
