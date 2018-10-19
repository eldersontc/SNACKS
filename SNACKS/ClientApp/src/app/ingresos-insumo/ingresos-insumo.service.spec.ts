import { TestBed } from '@angular/core/testing';

import { IngresosInsumoService } from './ingresos-insumo.service';

describe('IngresosInsumoService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: IngresosInsumoService = TestBed.get(IngresosInsumoService);
    expect(service).toBeTruthy();
  });
});
