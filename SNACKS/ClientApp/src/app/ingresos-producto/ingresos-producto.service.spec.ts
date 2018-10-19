import { TestBed } from '@angular/core/testing';

import { IngresosProductoService } from './ingresos-producto.service';

describe('IngresosProductoService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: IngresosProductoService = TestBed.get(IngresosProductoService);
    expect(service).toBeTruthy();
  });
});
