import { Component, OnInit, ChangeDetectorRef, Inject, PLATFORM_ID } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { RouterModule } from '@angular/router';
import { WishlistService, WishlistItem } from '../../services/wishlist';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-wishlist',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './wishlist.html',
  styleUrls: ['./wishlist.css']
})
export class WishlistComponent implements OnInit {
  wishlist: WishlistItem[] = [];
  isLoading = true;
  isRemoving: Record<number, boolean> = {};
  error = '';

  constructor(
    private wishlistService: WishlistService,
    private auth: AuthService,
    private cdr: ChangeDetectorRef,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}

  ngOnInit() {
    if (isPlatformBrowser(this.platformId)) {
        if (this.auth.isLoggedIn() && this.auth.isRenter()) {
          this.loadWishlist();
        } else {
          this.isLoading = false;
          this.error = 'You must be logged in as a Renter to view your wishlist.';
          this.cdr.detectChanges();
        }
    } else {
        
        this.isLoading = false;
    }
  }

  loadWishlist() {
    this.isLoading = true;
    this.wishlistService.getMyWishlist().subscribe({
      next: (data) => {
        this.wishlist = data;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Failed to load wishlist:', err);
        this.error = 'Could not load your saved properties.';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  removeFromWishlist(propertyId: number, event: Event) {
    if (this.isRemoving[propertyId]) return;
    event.stopPropagation();
    event.preventDefault();
    this.isRemoving[propertyId] = true;
    this.wishlistService.removeFromWishlist(propertyId).subscribe({
      next: () => {
        this.isRemoving[propertyId] = false;
        this.wishlist = this.wishlist.filter(w => w.propertyId !== propertyId);
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.isRemoving[propertyId] = false;
        console.error('Failed to remove from wishlist:', err);
        this.cdr.detectChanges();
      }
    });
  }
}
