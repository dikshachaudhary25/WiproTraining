import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PLATFORM_ID } from '@angular/core';
import { PropertyDetailsComponent } from './property-details';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ActivatedRoute, Router } from '@angular/router';
import { of, BehaviorSubject } from 'rxjs';
import { vi } from 'vitest';
import { PropertyService } from '../../services/property';
import { ReservationService } from '../../services/reservation';
import { AuthService } from '../../services/auth';
import { MessageService } from '../../services/message';
import { ReviewService } from '../../services/review';
import { WishlistService } from '../../services/wishlist';
import { SignalRService } from '../../services/signalr.service';
import { provideRouter, convertToParamMap } from '@angular/router';

describe('PropertyDetailsComponent', () => {
  let component: PropertyDetailsComponent;
  let fixture: ComponentFixture<PropertyDetailsComponent>;

  beforeEach(async () => {
    const propertyServiceMock = {
      getPropertyById: vi.fn().mockReturnValue(of({ 
        propertyId: 1, 
        title: 'Test Prop', 
        images: [], 
        ownerId: 100,
        location: 'Test Loc',
        city: 'Test City',
        state: 'Test State',
        pricePerNight: 100
      }))
    };
    const reservationServiceMock = {
      getBookedDates: vi.fn().mockReturnValue(of([])),
      createReservation: vi.fn().mockReturnValue(of({ success: true }))
    };
    const authServiceMock = {
      isLoggedIn: vi.fn().mockReturnValue(true),
      isRenter: vi.fn().mockReturnValue(true),
      isOwner: vi.fn().mockReturnValue(false),
      getToken: vi.fn().mockReturnValue('fake-token'),
      getUserId: vi.fn().mockReturnValue(1)
    };
    const messageServiceMock = {
      getConversation: vi.fn().mockReturnValue(of([])),
      sendMessage: vi.fn().mockReturnValue(of({}))
    };
    const reviewServiceMock = {
      getReviewsByProperty: vi.fn().mockReturnValue(of([]))
    };
    const wishlistServiceMock = {
      getMyWishlist: vi.fn().mockReturnValue(of([])),
      addToWishlist: vi.fn().mockReturnValue(of({})),
      removeFromWishlist: vi.fn().mockReturnValue(of({}))
    };
    const signalRServiceMock = {
      startConnection: vi.fn(),
      onMessageReceived: vi.fn()
    };

    await TestBed.configureTestingModule({
      imports: [PropertyDetailsComponent, HttpClientTestingModule],
      providers: [
        { provide: PropertyService, useValue: propertyServiceMock },
        { provide: ReservationService, useValue: reservationServiceMock },
        { provide: AuthService, useValue: authServiceMock },
        { provide: MessageService, useValue: messageServiceMock },
        { provide: ReviewService, useValue: reviewServiceMock },
        { provide: WishlistService, useValue: wishlistServiceMock },
        { provide: SignalRService, useValue: signalRServiceMock },
        { provide: PLATFORM_ID, useValue: 'browser' },
        provideRouter([]),
        {
          provide: ActivatedRoute,
          useValue: {
            paramMap: new BehaviorSubject(convertToParamMap({ id: '1' }))
          }
        }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(PropertyDetailsComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load property details on init', async () => {
    const getPropertySpy = vi.spyOn(TestBed.inject(PropertyService), 'getPropertyById');
    
    fixture.detectChanges();
    await fixture.whenStable();
    
    expect(getPropertySpy).toHaveBeenCalledWith(1);
    expect(component.property).toBeTruthy();
    expect(component.property?.propertyId).toBe(1);
    expect(component.isLoading).toBe(false);
  });

  it('should handle property reservation flow', () => {
    component.property = { propertyId: 1 } as any;
    component.checkInDate = '2026-04-01';
    component.checkOutDate = '2026-04-05';
    
    component.reserveProperty();
    
    expect(component.reservationSuccess).toBe('Reservation created successfully. Status: Pending.');
    expect(component.isReserving).toBe(false);
  });

  it('should validate dates before reservation', () => {
    component.property = { propertyId: 1 } as any;
    component.checkInDate = '2026-04-05';
    component.checkOutDate = '2026-04-01'; 
    
    component.reserveProperty();
    
    expect(component.reservationError).toBe('Check-out date must be after check-in date.');
  });
});
