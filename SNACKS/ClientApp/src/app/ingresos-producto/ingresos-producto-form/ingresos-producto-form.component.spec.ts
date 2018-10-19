import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IngresosProductoFormComponent } from './ingresos-producto-form.component';

describe('IngresosProductoFormComponent', () => {
  let component: IngresosProductoFormComponent;
  let fixture: ComponentFixture<IngresosProductoFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IngresosProductoFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IngresosProductoFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
