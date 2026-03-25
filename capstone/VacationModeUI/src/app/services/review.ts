import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth';

export interface Review {
  reviewId?: number;
  userId?: number;
  propertyId: number;
  rating: number;
  comment: string;
  userName?: string;
  createdAt?: string;
  user?: {
    userId: number;
    fullName: string;
  };
}

@Injectable({
  providedIn: 'root'
})
export class ReviewService {
  private apiUrl = '/api/review';

  constructor(private http: HttpClient, private auth: AuthService) {}

  private getAuthHeaders(): HttpHeaders {
    const token = this.auth.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  getReviewsByProperty(propertyId: number): Observable<Review[]> {
    return this.http.get<Review[]>(`${this.apiUrl}/property/${propertyId}`);
  }

  createReview(dto: { propertyId: number; rating: number; comment: string }): Observable<Review> {
    return this.http.post<Review>(this.apiUrl, dto, { headers: this.getAuthHeaders() });
  }

  getMyReviews(): Observable<Review[]> {
    return this.http.get<Review[]>(`${this.apiUrl}/my-reviews`, { headers: this.getAuthHeaders() });
  }
}
