import { TestBed } from '@angular/core/testing';

import { LotesService } from './lotes.service';

describe('LotesService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: LotesService = TestBed.get(LotesService);
    expect(service).toBeTruthy();
  });
});
