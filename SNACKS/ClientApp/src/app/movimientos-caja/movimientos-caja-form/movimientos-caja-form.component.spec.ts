import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MovimientosCajaFormComponent } from './movimientos-caja-form.component';

describe('MovimientosCajaFormComponent', () => {
  let component: MovimientosCajaFormComponent;
  let fixture: ComponentFixture<MovimientosCajaFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MovimientosCajaFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MovimientosCajaFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
