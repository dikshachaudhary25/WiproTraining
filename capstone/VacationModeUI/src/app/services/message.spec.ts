import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { MessageService, Message, SendMessageDto } from './message';
import { AuthService } from './auth';
import { vi } from 'vitest';

describe('MessageService', () => {
  let service: MessageService;
  let httpMock: HttpTestingController;
  let authService: AuthService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [MessageService, AuthService]
    });
    service = TestBed.inject(MessageService);
    httpMock = TestBed.inject(HttpTestingController);
    authService = TestBed.inject(AuthService);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should send a message with auth headers', () => {
    const dto: SendMessageDto = { propertyId: 1, messageText: 'Hello', receiverId: 2 };
    vi.spyOn(authService, 'getToken').mockReturnValue('fake-token');

    service.sendMessage(dto).subscribe(res => {
      expect(res).toBeTruthy();
    });

    const req = httpMock.expectOne('/api/message');
    expect(req.request.method).toBe('POST');
    expect(req.request.headers.get('Authorization')).toBe('Bearer fake-token');
    req.flush({ success: true });
  });

  it('should get my messages', () => {
    service.getMyMessages().subscribe(res => {
      expect(res.length).toBe(0);
    });

    const req = httpMock.expectOne('/api/message/my-messages');
    expect(req.request.method).toBe('GET');
    req.flush([]);
  });

  it('should get property messages', () => {
    vi.spyOn(authService, 'getToken').mockReturnValue('fake-token');

    service.getPropertyMessages(1).subscribe(res => {
        expect(res.length).toBe(0);
    });

    const req = httpMock.expectOne('/api/message/property/1');
    expect(req.request.method).toBe('GET');
    expect(req.request.headers.get('Authorization')).toBe('Bearer fake-token');
    req.flush([]);
  });

  it('should get a conversation', () => {
    vi.spyOn(authService, 'getToken').mockReturnValue('fake-token');

    service.getConversation(1, 2).subscribe(res => {
      expect(res.length).toBe(0);
    });

    const req = httpMock.expectOne('/api/message/conversation/1/2');
    expect(req.request.method).toBe('GET');
    req.flush([]);
  });
});
