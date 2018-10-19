import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IngresosProductoComponent } from './ingresos-producto.component';

describe('IngresosProductoComponent', () => {
  let component: IngresosProductoComponent;
  let fixture: ComponentFixture<IngresosProductoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IngresosProductoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IngresosProductoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
