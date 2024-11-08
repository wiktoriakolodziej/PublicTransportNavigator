import { TestBed } from '@angular/core/testing';

import { UserTravelService } from './user-travel.service';

describe('UserTravelService', () => {
  let service: UserTravelService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UserTravelService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
