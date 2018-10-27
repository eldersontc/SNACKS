import { TestBed } from '@angular/core/testing';

import { ZonasVentaService } from './zonas-venta.service';

describe('ZonasVentaService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ZonasVentaService = TestBed.get(ZonasVentaService);
    expect(service).toBeTruthy();
  });
});
