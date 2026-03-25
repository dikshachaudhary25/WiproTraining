import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { WishlistService, WishlistItem } from './wishlist';
import { AuthService } from './auth';
import { vi } from 'vitest';

describe('WishlistService', () => {
  let service: WishlistService;
  let httpMock: HttpTestingController;
  let authService: AuthService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [WishlistService, AuthService]
    });
    service = TestBed.inject(WishlistService);
    httpMock = TestBed.inject(HttpTestingController);
    authService = TestBed.inject(AuthService);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should add to wishlist with auth headers', () => {
    vi.spyOn(authService, 'getToken').mockReturnValue('fake-token');

    service.addToWishlist(101).subscribe(res => {
      expect(res).toBeTruthy();
    });

    const req = httpMock.expectOne('/api/wishlist/add/101');
    expect(req.request.method).toBe('POST');
    expect(req.request.headers.get('Authorization')).toBe('Bearer fake-token');
    req.flush({ success: true });
  });

  it('should remove from wishlist with auth headers', () => {
    vi.spyOn(authService, 'getToken').mockReturnValue('fake-token');

    service.removeFromWishlist(101).subscribe(res => {
      expect(res).toBeTruthy();
    });

    const req = httpMock.expectOne('/api/wishlist/remove/101');
    expect(req.request.method).toBe('DELETE');
    expect(req.request.headers.get('Authorization')).toBe('Bearer fake-token');
    req.flush({ success: true });
  });

  it('should retrieve my wishlist', () => {
    const dummyWishlist: WishlistItem[] = [
      { wishlistId: 1, propertyId: 101, property: { propertyId: 101, title: 'Prop 1', city: 'City', state: 'State', pricePerNight: 100, images: [], rating: 5 } }
    ];

    vi.spyOn(authService, 'getToken').mockReturnValue('fake-token');

    service.getMyWishlist().subscribe(list => {
      expect(list.length).toBe(1);
      expect(list).toEqual(dummyWishlist);
    });

    const req = httpMock.expectOne('/api/wishlist');
    expect(req.request.method).toBe('GET');
    req.flush(dummyWishlist);
  });
});
