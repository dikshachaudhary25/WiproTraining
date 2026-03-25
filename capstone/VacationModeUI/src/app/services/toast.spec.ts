import { TestBed } from '@angular/core/testing';
import { ToastService, ToastMessage } from './toast';
import { vi } from 'vitest';

describe('ToastService', () => {
  let service: ToastService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ToastService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should emit a toast message when show() is called', () => {
    let emittedMessage: ToastMessage | undefined;
    service.toastState.subscribe(msg => emittedMessage = msg);

    service.show('Test Message', 'success');

    expect(emittedMessage).toEqual({ text: 'Test Message', type: 'success' });
  });

  it('should emit an error toast message when showError() is called', () => {
    let emittedMessage: ToastMessage | undefined;
    service.toastState.subscribe(msg => emittedMessage = msg);

    service.showError('Error Message');

    expect(emittedMessage).toEqual({ text: 'Error Message', type: 'error' });
  });

  it('should emit a success toast message when showSuccess() is called', () => {
    let emittedMessage: ToastMessage | undefined;
    service.toastState.subscribe(msg => emittedMessage = msg);

    service.showSuccess('Success Message');

    expect(emittedMessage).toEqual({ text: 'Success Message', type: 'success' });
  });

  it('should default to type info when show() is called without type', () => {
    let emittedMessage: ToastMessage | undefined;
    service.toastState.subscribe(msg => emittedMessage = msg);

    service.show('Info Message');

    expect(emittedMessage).toEqual({ text: 'Info Message', type: 'info' });
  });
});
