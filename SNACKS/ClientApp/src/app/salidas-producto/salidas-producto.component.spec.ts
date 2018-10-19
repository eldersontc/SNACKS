import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SalidasProductoComponent } from './salidas-producto.component';

describe('SalidasProductoComponent', () => {
  let component: SalidasProductoComponent;
  let fixture: ComponentFixture<SalidasProductoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SalidasProductoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SalidasProductoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
