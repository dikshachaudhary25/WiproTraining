import { Component, OnInit, ChangeDetectorRef, Inject, PLATFORM_ID } from '@angular/core';
import { CommonModule, isPlatformBrowser, DecimalPipe } from '@angular/common';
import { finalize } from 'rxjs/operators';
import { FormsModule } from '@angular/forms';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { PropertyService } from '../../services/property';
import { ReservationService } from '../../services/reservation';
import { AuthService } from '../../services/auth';
import { WishlistService } from '../../services/wishlist';
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
  isLoading = false;
  deleteError = '';
  wishlistIds: number[] = [];

  
  isDeleting: Record<number, boolean> = {};
  isReserving: Record<number, boolean> = {};
  isWishlisting: Record<number, boolean> = {};
  placeholderImage = 'data:image/svg+xml;utf8,<svg xmlns="http://www.w3.org/2000/svg" width="800" height="500"><rect width="100%" height="100%" fill="%23e9eefb"/><text x="50%" y="50%" dominant-baseline="middle" text-anchor="middle" fill="%236a7693" font-family="Arial" font-size="28">VacationMode</text></svg>';

  searchCity = '';
  searchType = '';
  searchMinPrice: number | null = null;
  searchMaxPrice: number | null = null;
  sortBy: 'rating' | 'price' = 'rating';

  searchCheckIn = '';
  searchCheckOut = '';
  searchFeatures = '';

  
  reservingPropertyId: number | null = null;
  checkInDate = '';
  checkOutDate = '';
  reservationSuccess = '';
  reservationError = '';
  readonly today = new Date().toISOString().split('T')[0];

  constructor(
    private propertyService: PropertyService,
    private reservationService: ReservationService,
    private wishlistService: WishlistService,
    public auth: AuthService,
    private router: Router,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}

  ngOnInit() {
    if (isPlatformBrowser(this.platformId)) {
      this.route.queryParams.subscribe(params => {
        console.log('[Properties] Query parameters detected:', params);
        if (params['city']) this.searchCity = params['city'];
        if (params['type']) this.searchType = params['type'];
        
        if (this.isFilterApplied()) {
          this.executeSearch();
        } else {
          this.loadAllProperties();
        }
        
        if (this.auth.isLoggedIn() && this.auth.isRenter()) {
          this.loadWishlist();
        }
      });
    }
  }

  loadWishlist() {
    this.wishlistService.getMyWishlist().subscribe({
      next: (data) => {
        this.wishlistIds = data.map(item => item.propertyId);
        this.syncWishlistState();
      },
      error: (err) => console.error('Failed to load wishlist:', err)
    });
  }

  syncWishlistState() {
    this.properties.forEach(p => {
      if (p.propertyId) {
        p.isInWishlist = this.wishlistIds.includes(p.propertyId);
      }
    });
    this.cdr.detectChanges();
  }

  isFilterApplied(): boolean {
    return !!(
      this.searchCity || 
      this.searchType || 
      this.searchMinPrice || 
      this.searchMaxPrice || 
      this.searchCheckIn || 
      this.searchCheckOut || 
      this.searchFeatures
    );
  }

  loadAllProperties() {
    this.isLoading = true;
    console.log('[Properties] Executing loadAllProperties()');
    this.propertyService.getAllProperties().pipe(
      finalize(() => {
        this.isLoading = false;
        this.cdr.detectChanges();
      })
    ).subscribe({
      next: (data: Property[]) => {
        console.log('[Properties] Received properties count:', data.length);
        this.properties = data;
        this.filteredProperties = data;
        this.syncWishlistState();
        this.sortProperties();
      },
      error: (err) => {
        console.error('[Properties] loadAllProperties failed:', err);
      }
    });
  }

  executeSearch() {
    this.isLoading = true;
    this.deleteError = '';

    if (!this.isFilterApplied()) {
      this.loadAllProperties();
      return;
    }

    const params: any = {};
    if (this.searchCity) params.city = this.searchCity;
    if (this.searchType) params.type = this.searchType;
    if (this.searchMinPrice) params.minPrice = this.searchMinPrice;
    if (this.searchMaxPrice) params.maxPrice = this.searchMaxPrice;
    if (this.searchCheckIn && this.searchCheckOut) {
      params.checkIn = this.searchCheckIn;
      params.checkOut = this.searchCheckOut;
    }
    if (this.searchFeatures) params.features = this.searchFeatures;

    console.log("[Properties] Filters being sent to Search API:", params);

    this.propertyService.searchProperties(params).pipe(
      finalize(() => {
        this.isLoading = false;
        this.cdr.detectChanges();
      })
    ).subscribe({
      next: (data: Property[]) => {
        console.log('[Properties] Search results count:', data.length);
        this.properties = data;
        this.filteredProperties = data;
        this.syncWishlistState();
        this.sortProperties();
      },
      error: (err) => {
        console.error('[Properties] Search failed:', err);
      }
    });
  }

  clearFilters() {
    this.searchCity = '';
    this.searchType = '';
    this.searchMinPrice = null;
    this.searchMaxPrice = null;
    this.searchCheckIn = '';
    this.searchCheckOut = '';
    this.searchFeatures = '';
    
    
    this.loadAllProperties();
  }

  loadProperties() {
    this.loadAllProperties();
  }

  filterProperties() {
    this.executeSearch();
  }

  sortProperties() {
    if (this.sortBy === 'rating') {
      this.filteredProperties.sort((a, b) => (b.rating || 0) - (a.rating || 0));
    } else if (this.sortBy === 'price') {
      this.filteredProperties.sort((a, b) => a.pricePerNight - b.pricePerNight);
    }
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

    this.isDeleting[id] = true;
    this.propertyService.deleteProperty(id).subscribe({
      next: () => {
        this.isDeleting[id] = false;
        this.executeSearch();
      },
      error: () => {
        this.isDeleting[id] = false;
        this.deleteError = 'Failed to delete property. Please try again.';
      }
    });
  }

  addToWishlist(id: number | undefined, event: Event) {
    if (id === undefined) return;
    event.stopPropagation();
    event.preventDefault();

    if (!this.auth.isLoggedIn()) {
      alert("Please login to save to your wishlist.");
      return;
    }

    if (this.isWishlisting[id]) return;
    this.isWishlisting[id] = true;

    const property = this.properties.find(p => p.propertyId === id);
    if (property?.isInWishlist) {
      this.wishlistService.removeFromWishlist(id).subscribe({
        next: () => {
          property.isInWishlist = false;
          this.wishlistIds = this.wishlistIds.filter(wid => wid !== id);
          this.isWishlisting[id] = false;
        },
        error: (err) => {
          this.isWishlisting[id] = false;
          alert("Failed to remove from wishlist.");
        }
      });
      return;
    }

    this.wishlistService.addToWishlist(id).subscribe({
      next: () => {
        if (property) property.isInWishlist = true;
        this.wishlistIds.push(id);
        this.isWishlisting[id] = false;
      },
      error: (err) => {
        this.isWishlisting[id] = false;
        const msg = err?.error?.message || err?.error || "Failed to add to wishlist.";
        alert(msg);
      }
    });
  }

  
  isPropertyOwner(property: Property): boolean {
    return this.auth.isLoggedIn()
      && this.auth.isOwner()
      && property.ownerId === this.auth.getUserId();
  }

  
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

    this.isReserving[propertyId] = true;
    this.reservationService.createReservation({
      propertyId,
      checkInDate: this.checkInDate,
      checkOutDate: this.checkOutDate
    }).subscribe({
      next: () => {
        this.isReserving[propertyId] = false;
        this.reservationSuccess = 'Reservation created! Status: Pending.';
        this.reservationError = '';
        this.checkInDate = '';
        this.checkOutDate = '';
      },
      error: (err) => {
        this.isReserving[propertyId] = false;
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