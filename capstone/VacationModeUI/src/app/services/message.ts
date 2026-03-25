import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth';

export interface SendMessageDto {
  propertyId: number;
  messageText: string;
  receiverId?: number;
}

export interface Message {
  messageId: number;
  senderId: number;
  senderName?: string;
  receiverId: number;
  receiverName?: string;
  propertyId: number;
  propertyTitle?: string;
  messageText: string;
  sentAt: Date;
  isRead: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  private apiUrl = '/api/message';

  constructor(private http: HttpClient, private auth: AuthService) {}

  private getAuthHeaders(): HttpHeaders {
    const token = this.auth.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  sendMessage(dto: SendMessageDto): Observable<any> {
    return this.http.post<any>(this.apiUrl, dto, { headers: this.getAuthHeaders() });
  }

  getMyMessages(): Observable<Message[]> {
    return this.http.get<Message[]>(`${this.apiUrl}/my-messages`);
  }

  getPropertyMessages(propertyId: number): Observable<Message[]> {
    return this.http.get<Message[]>(`${this.apiUrl}/property/${propertyId}`, { headers: this.getAuthHeaders() });
  }

  getConversation(propertyId: number, otherUserId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/conversation/${propertyId}/${otherUserId}`, { headers: this.getAuthHeaders() });
  }
}
