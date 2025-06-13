import { TestBed } from '@angular/core/testing';

import { AiModelService } from './ai-model.service';

describe('AiModelService', () => {
  let service: AiModelService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AiModelService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
