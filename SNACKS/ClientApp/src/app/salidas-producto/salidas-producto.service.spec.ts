import { TestBed } from '@angular/core/testing';

import { SalidasProductoService } from './salidas-producto.service';

describe('SalidasProductoService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SalidasProductoService = TestBed.get(SalidasProductoService);
    expect(service).toBeTruthy();
  });
});
