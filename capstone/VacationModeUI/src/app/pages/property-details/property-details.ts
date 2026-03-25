import { Component, OnInit, ChangeDetectorRef, Inject, PLATFORM_ID } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { PropertyService } from '../../services/property';
import { ReservationService } from '../../services/reservation';
import { AuthService } from '../../services/auth';
import { MessageService } from '../../services/message';
import { ReviewService, Review } from '../../services/review';
import { WishlistService } from '../../services/wishlist';
import { Property } from '../../models/property.model';
import { SignalRService } from '../../services/signalr.service';

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
  isReserving = false;
  isWishlisting = false;
  isSubmittingReview = false;

  checkInDate = '';
  checkOutDate = '';

  reservationSuccess = '';
  reservationError = '';

  selectedImage: string | null = null;
  messageText = '';
  messageSuccess = '';
  messageError = '';
  isSendingMessage = false;
  conversation: any[] = [];
  bookedDates: string[] = [];

  reviews: Review[] = [];
  newReviewRating = 5;
  newReviewComment = '';
  reviewSuccess = '';
  reviewError = '';

  readonly today = new Date().toISOString().split('T')[0];

  readonly placeholderImage =
    'data:image/svg+xml;utf8,<svg xmlns="http://www.w3.org/2000/svg" width="1200" height="700"><rect width="100%25" height="100%25" fill="%23e9eefb"/><text x="50%25" y="50%25" dominant-baseline="middle" text-anchor="middle" fill="%236a7693" font-family="Arial" font-size="36">VacationMode Property</text></svg>';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private propertyService: PropertyService,
    private reservationService: ReservationService,
    public auth: AuthService,
    private messageService: MessageService,
    private reviewService: ReviewService,
    private wishlistService: WishlistService,
    private signalRService: SignalRService,
    private cdr: ChangeDetectorRef,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {
    
  }

  ngOnInit() {
    if (isPlatformBrowser(this.platformId)) {
      this.route.paramMap.subscribe(params => {
        const idParam = params.get('id');
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
    } else {
      this.isLoading = false;
    }
  }

  loadProperty(id: number) {

    this.isLoading = true;

    this.propertyService.getPropertyById(id).subscribe({
      next: (data: Property) => {
        this.property = data;
        if (data.images && data.images.length > 0) {
          this.selectedImage = data.images[0];
        } else {
            this.selectedImage = null;
        }
        
        
        this.reviewService.getReviewsByProperty(id).subscribe({
          next: (revs) => {
            this.reviews = revs;
            this.isLoading = false;
            this.cdr.detectChanges();
          },
          error: (err) => {
            console.error('Failed to load reviews:', err);
            this.isLoading = false;
            this.cdr.detectChanges();
          }
        });

        
        this.reservationService.getBookedDates(id).subscribe({
          next: (dates: string[]) => {
            this.bookedDates = dates;
          },
          error: (err: any) => console.error('Failed to load booked dates', err)
        });

        if (this.auth.isLoggedIn() && this.auth.isRenter()) {
          this.loadConversation();
          this.checkWishlistStatus();
        }
        
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

    
    let isOverlap = false;
    let currentCheckDate = new Date(this.checkInDate);
    const endCheckDate = new Date(this.checkOutDate);

    while (currentCheckDate < endCheckDate) {
      const dateString = currentCheckDate.toISOString().split('T')[0];
      if (this.bookedDates.includes(dateString)) {
        isOverlap = true;
        break;
      }
      currentCheckDate.setDate(currentCheckDate.getDate() + 1);
    }

    if (isOverlap) {
      this.reservationError = 'These dates are already booked. Please select available dates.';
      return;
    }

    this.isReserving = true;
    this.reservationService.createReservation({
      propertyId: this.property.propertyId!,
      checkInDate: this.checkInDate,
      checkOutDate: this.checkOutDate
    }).subscribe({
      next: () => {
        this.isReserving = false;
        this.reservationSuccess = 'Reservation created successfully. Status: Pending.';
        this.reservationError = '';
        this.checkInDate = '';
        this.checkOutDate = '';
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.isReserving = false;
        console.error('Reservation failed:', err);
        this.reservationError = err?.error || 'Reservation failed. Please try again.';
        this.reservationSuccess = '';
        this.cdr.detectChanges();
      }
    });
  }

  loadConversation() {
    if (!this.property || !this.property.ownerId || !this.auth.isLoggedIn()) return;
    
    
    const otherUserId = this.property.ownerId;
    if (otherUserId === undefined) return;

    this.messageService.getConversation(this.property.propertyId!, otherUserId).subscribe({
      next: (data: any[]) => {
        this.conversation = data;
        
        
        const token = this.auth.getToken();
        if (token) {
          this.signalRService.startConnection(token);
          this.signalRService.onMessageReceived((senderId: string, messageDto: any) => {
            
            if (messageDto.propertyId === this.property?.propertyId && 
               (messageDto.senderId === otherUserId || messageDto.receiverId === otherUserId)) {
               
               if (messageDto.senderId !== this.auth.getUserId()) {
                 this.conversation.push(messageDto);
                 this.cdr.detectChanges();
               }
            }
          });
        }
        
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        console.error('Failed to load conversation:', err);
      }
    });
  }

  sendMessage() {
    if (!this.property?.propertyId || !this.messageText.trim()) return;

    this.messageSuccess = '';
    this.messageError = '';
    this.isSendingMessage = true;
    const textToSend = this.messageText;

    this.messageService.sendMessage({
      propertyId: this.property.propertyId!,
      messageText: textToSend
    }).subscribe({
      next: (newMsg) => {
        this.messageSuccess = 'Message sent!';
        this.messageText = '';
        this.isSendingMessage = false;
        
        
        this.conversation.push({
          messageId: Date.now(),
          propertyId: this.property!.propertyId!,
          senderId: this.auth.getUserId(),
          receiverId: this.property!.ownerId!,
          messageText: textToSend,
          sentAt: new Date().toISOString()
        });

        
        setTimeout(() => { this.messageSuccess = ''; this.cdr.detectChanges(); }, 3000);
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Message failed:', err);
        this.messageError = 'Failed to send message.';
        this.isSendingMessage = false;
        this.cdr.detectChanges();
      }
    });
  }

  submitReview() {
    if (!this.property?.propertyId || !this.newReviewComment.trim()) return;

    this.reviewSuccess = '';
    this.reviewError = '';

    if (!this.auth.isLoggedIn()) {
      this.reviewError = 'Please log in to leave a review.';
      return;
    }

    this.isSubmittingReview = true;
    this.reviewService.createReview({
      propertyId: this.property.propertyId!,
      rating: this.newReviewRating,
      comment: this.newReviewComment
    }).subscribe({
      next: (review) => {
        this.isSubmittingReview = false;
        this.reviewSuccess = 'Review submitted successfully!';
        this.newReviewComment = '';
        this.newReviewRating = 5;
        this.loadProperty(this.property!.propertyId!);
      },
      error: (err) => {
        this.isSubmittingReview = false;
        this.reviewError = err?.error || 'Failed to submit review.';
        this.cdr.detectChanges();
      }
    });
  }

  checkWishlistStatus() {
    if (!this.property?.propertyId) return;
    this.wishlistService.getMyWishlist().subscribe({
      next: (wishlist) => {
        if (this.property) {
          this.property.isInWishlist = wishlist.some(w => w.propertyId === this.property?.propertyId);
          this.cdr.detectChanges();
        }
      }
    });
  }

  toggleWishlist(event: Event) {
    if (!this.property?.propertyId) return;
    event.stopPropagation();

    if (!this.auth.isLoggedIn()) {
      alert("Please login to save to your wishlist.");
      return;
    }

    if (this.isWishlisting) return;
    this.isWishlisting = true;

    if (this.property.isInWishlist) {
      this.wishlistService.removeFromWishlist(this.property.propertyId).subscribe({
        next: () => {
          this.isWishlisting = false;
          if (this.property) this.property.isInWishlist = false;
          alert("Removed from wishlist!");
          this.cdr.detectChanges();
        },
        error: (err) => {
          this.isWishlisting = false;
          alert("Failed to remove from wishlist.");
        }
      });
    } else {
      this.wishlistService.addToWishlist(this.property.propertyId).subscribe({
        next: () => {
          this.isWishlisting = false;
          if (this.property) this.property.isInWishlist = true;
          alert("Added to wishlist! ❤️");
          this.cdr.detectChanges();
        },
        error: (err) => {
          this.isWishlisting = false;
          const msg = err?.error?.message || err?.error || "Failed to add to wishlist.";
          alert(msg);
        }
      });
    }
  }
}