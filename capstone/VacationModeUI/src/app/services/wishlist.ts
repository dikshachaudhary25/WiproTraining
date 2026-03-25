import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth';

export interface WishlistItem {
  wishlistId: number;
  propertyId: number;
  property?: {
    propertyId: number;
    title: string;
    city: string;
    state: string;
    pricePerNight: number;
    images: string[];
    rating: number;
  };
}

@Injectable({
  providedIn: 'root'
})
export class WishlistService {
  private apiUrl = '/api/wishlist';

  constructor(private http: HttpClient, private auth: AuthService) {}

  private getAuthHeaders(): HttpHeaders {
    const token = this.auth.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  addToWishlist(propertyId: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/add/${propertyId}`, {}, { headers: this.getAuthHeaders() });
  }

  removeFromWishlist(propertyId: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/remove/${propertyId}`, { headers: this.getAuthHeaders() });
  }

  getMyWishlist(): Observable<WishlistItem[]> {
    return this.http.get<WishlistItem[]>(this.apiUrl, { headers: this.getAuthHeaders() });
  }
}
