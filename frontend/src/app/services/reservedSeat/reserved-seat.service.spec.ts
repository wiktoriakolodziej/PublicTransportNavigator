import { TestBed } from '@angular/core/testing';

import { ReservedSeatService } from './reserved-seat.service';

describe('ReservedSeatService', () => {
  let service: ReservedSeatService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ReservedSeatService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
