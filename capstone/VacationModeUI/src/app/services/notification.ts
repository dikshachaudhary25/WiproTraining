import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth';

export interface AppNotification {
  notificationId: number;
  userId: number;
  message: string;
  isRead: boolean;
  type?: string;
  referenceId?: number;
  createdAt: string;
}

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private apiUrl = '/api/notification';

  constructor(private http: HttpClient, private auth: AuthService) {}

  private getAuthHeaders(): HttpHeaders {
    const token = this.auth.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  getMyNotifications(): Observable<AppNotification[]> {
    return this.http.get<AppNotification[]>(this.apiUrl, { headers: this.getAuthHeaders() });
  }

  markAsRead(id: number): Observable<any> {
    return this.http.patch(`${this.apiUrl}/read/${id}`, {}, { headers: this.getAuthHeaders() });
  }

  markAllAsRead(): Observable<any> {
    return this.http.patch(`${this.apiUrl}/read-all`, {}, { headers: this.getAuthHeaders() });
  }
}
