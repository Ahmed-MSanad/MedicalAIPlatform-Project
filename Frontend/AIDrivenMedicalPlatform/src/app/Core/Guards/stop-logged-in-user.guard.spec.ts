import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { stopLoggedInUserGuard } from './stop-logged-in-user.guard';

describe('stopLoggedInUserGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => stopLoggedInUserGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
