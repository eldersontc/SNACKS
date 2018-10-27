import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ZonasVentaComponent } from './zonas-venta.component';

describe('ZonasVentaComponent', () => {
  let component: ZonasVentaComponent;
  let fixture: ComponentFixture<ZonasVentaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ZonasVentaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ZonasVentaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
