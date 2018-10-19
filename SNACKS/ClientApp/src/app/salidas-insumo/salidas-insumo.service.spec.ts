import { TestBed } from '@angular/core/testing';

import { SalidasInsumoService } from './salidas-insumo.service';

describe('SalidasInsumoService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SalidasInsumoService = TestBed.get(SalidasInsumoService);
    expect(service).toBeTruthy();
  });
});
