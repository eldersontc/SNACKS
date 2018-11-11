import { TestBed } from '@angular/core/testing';

import { MovimientosCajaService } from './movimientos-caja.service';

describe('MovimientosCajaService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: MovimientosCajaService = TestBed.get(MovimientosCajaService);
    expect(service).toBeTruthy();
  });
});
