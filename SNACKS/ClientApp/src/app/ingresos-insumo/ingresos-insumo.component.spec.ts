import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IngresosInsumoComponent } from './ingresos-insumo.component';

describe('IngresosInsumoComponent', () => {
  let component: IngresosInsumoComponent;
  let fixture: ComponentFixture<IngresosInsumoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IngresosInsumoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IngresosInsumoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
