import { TestBed } from '@angular/core/testing';

import { Hobby } from './hobby';

describe('Hobby', () => {
  let service: Hobby;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Hobby);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
