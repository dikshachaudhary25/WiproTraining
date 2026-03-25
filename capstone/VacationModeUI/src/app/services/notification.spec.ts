import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { NotificationService, AppNotification } from './notification';
import { AuthService } from './auth';
import { vi } from 'vitest';

describe('NotificationService', () => {
  let service: NotificationService;
  let httpMock: HttpTestingController;
  let authService: AuthService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [NotificationService, AuthService]
    });
    service = TestBed.inject(NotificationService);
    httpMock = TestBed.inject(HttpTestingController);
    authService = TestBed.inject(AuthService);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should retrieve my notifications with auth headers', () => {
    const dummyNotifications: AppNotification[] = [
      { notificationId: 1, userId: 1, message: 'Test 1', isRead: false, createdAt: new Date().toISOString() },
      { notificationId: 2, userId: 1, message: 'Test 2', isRead: true, createdAt: new Date().toISOString() }
    ];

    vi.spyOn(authService, 'getToken').mockReturnValue('fake-token');

    service.getMyNotifications().subscribe(notifications => {
      expect(notifications.length).toBe(2);
      expect(notifications).toEqual(dummyNotifications);
    });

    const req = httpMock.expectOne('/api/notification');
    expect(req.request.method).toBe('GET');
    expect(req.request.headers.get('Authorization')).toBe('Bearer fake-token');
    req.flush(dummyNotifications);
  });

  it('should mark a notification as read with auth headers', () => {
    vi.spyOn(authService, 'getToken').mockReturnValue('fake-token');

    service.markAsRead(1).subscribe(res => {
      expect(res).toBeTruthy();
    });

    const req = httpMock.expectOne('/api/notification/read/1');
    expect(req.request.method).toBe('PATCH');
    expect(req.request.headers.get('Authorization')).toBe('Bearer fake-token');
    req.flush({ success: true });
  });

  it('should mark all notifications as read', () => {
    vi.spyOn(authService, 'getToken').mockReturnValue('fake-token');

    service.markAllAsRead().subscribe(res => {
      expect(res).toBeTruthy();
    });

    const req = httpMock.expectOne('/api/notification/read-all');
    expect(req.request.method).toBe('PATCH');
    req.flush({ success: true });
  });
});
