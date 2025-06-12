import { TestBed } from '@angular/core/testing';

import { MedicalImageService } from './medical-image.service';

describe('MedicalImageService', () => {
  let service: MedicalImageService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MedicalImageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
