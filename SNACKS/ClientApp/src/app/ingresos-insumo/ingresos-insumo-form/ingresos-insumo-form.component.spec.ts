import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IngresosInsumoFormComponent } from './ingresos-insumo-form.component';

describe('IngresosInsumoFormComponent', () => {
  let component: IngresosInsumoFormComponent;
  let fixture: ComponentFixture<IngresosInsumoFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IngresosInsumoFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IngresosInsumoFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
