import { Component, OnInit, ChangeDetectorRef, Inject, PLATFORM_ID } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { ReservationService } from '../../services/reservation';
import { AuthService } from '../../services/auth';
import { Reservation } from '../../models/reservation.model';

@Component({
  selector: 'app-owner-reservations',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './owner-reservations.html',
  styleUrls: ['../my-reservations/my-reservations.css']
})
export class OwnerReservationsComponent implements OnInit {

  reservations: Reservation[] = [];
  isLoading = true;
  isConfirming: Record<number, boolean> = {};
  isRejecting: Record<number, boolean> = {};

  constructor(
    private reservationService: ReservationService,
    private auth: AuthService,
    private router: Router,
    private cdr: ChangeDetectorRef,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}

  ngOnInit() {
    if (!isPlatformBrowser(this.platformId)) {
      this.isLoading = false;
      return;
    }
    if (!this.auth.isLoggedIn() || !this.auth.isOwner()) {
      this.router.navigate(['/']);
      return;
    }
    this.loadReservations();
  }

  loadReservations() {
    this.isLoading = true;
    this.reservationService.getOwnerReservations().subscribe({
      next: (data) => {
        this.reservations = data;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Failed to load owner reservations:', err);
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  confirmReservation(id: number | undefined) {
    if (id === undefined || this.isConfirming[id]) return;
    this.isConfirming[id] = true;
    this.reservationService.confirmReservation(id).subscribe({
      next: () => {
        this.isConfirming[id] = false;
        this.loadReservations();
      },
      error: (err) => {
        console.error(err);
        this.isConfirming[id] = false;
        this.cdr.detectChanges();
      }
    });
  }

  rejectReservation(id: number | undefined) {
    if (id === undefined || this.isRejecting[id]) return;
    this.isRejecting[id] = true;
    this.reservationService.rejectReservation(id).subscribe({
      next: () => {
        this.isRejecting[id] = false;
        this.loadReservations();
      },
      error: (err) => {
        console.error(err);
        this.isRejecting[id] = false;
        this.cdr.detectChanges();
      }
    });
  }
}
