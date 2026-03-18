import { Injectable, inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';

const CLAIM_ROLE  = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';
const CLAIM_ID    = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier';
const CLAIM_NAME  = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name';
const CLAIM_EMAIL = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = '/api/auth';

  private platformId = inject(PLATFORM_ID);

  constructor(private http: HttpClient) {}

  register(data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, data, { responseType: 'text' });
  }

  login(data: any): Observable<string> {
    return this.http.post<any>(`${this.apiUrl}/login`, data).pipe(
      map(res => res.token || res.Token)
    );
  }

  saveToken(token: string) {

    if (!isPlatformBrowser(this.platformId)) return;

    try {

      const payload = JSON.parse(atob(token.split('.')[1]));

      localStorage.setItem('token', token);
      localStorage.setItem('userRole', payload[CLAIM_ROLE] || '');
      localStorage.setItem('userId', payload[CLAIM_ID] || '');
      localStorage.setItem('userName', payload[CLAIM_NAME] || '');
      localStorage.setItem('userEmail', payload[CLAIM_EMAIL] || '');

    } catch (err) {
      console.error('JWT parsing failed', err);
    }
  }

  getToken(): string | null {
    return isPlatformBrowser(this.platformId)
      ? localStorage.getItem('token')
      : null;
  }

  getUserRole(): string {
    return isPlatformBrowser(this.platformId)
      ? localStorage.getItem('userRole') || ''
      : '';
  }

  getUserId(): number {
    if (!isPlatformBrowser(this.platformId)) return 0;
    return parseInt(localStorage.getItem('userId') || '0', 10);
  }

  getUserName(): string {
    return isPlatformBrowser(this.platformId)
      ? localStorage.getItem('userName') || ''
      : '';
  }

  logout() {

    if (!isPlatformBrowser(this.platformId)) return;

    localStorage.removeItem('token');
    localStorage.removeItem('userRole');
    localStorage.removeItem('userId');
    localStorage.removeItem('userName');
    localStorage.removeItem('userEmail');
  }

  isLoggedIn(): boolean {
    return isPlatformBrowser(this.platformId) && !!localStorage.getItem('token');
  }

  isOwner(): boolean {
    return this.getUserRole() === 'Owner';
  }

  isRenter(): boolean {
    return this.getUserRole() === 'Renter';
  }
}