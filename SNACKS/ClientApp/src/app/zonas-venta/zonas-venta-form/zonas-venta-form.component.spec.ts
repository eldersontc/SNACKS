import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ZonasVentaFormComponent } from './zonas-venta-form.component';

describe('ZonasVentaFormComponent', () => {
  let component: ZonasVentaFormComponent;
  let fixture: ComponentFixture<ZonasVentaFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ZonasVentaFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ZonasVentaFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
