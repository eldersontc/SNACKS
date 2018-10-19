import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SalidasInsumoComponent } from './salidas-insumo.component';

describe('SalidasInsumoComponent', () => {
  let component: SalidasInsumoComponent;
  let fixture: ComponentFixture<SalidasInsumoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SalidasInsumoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SalidasInsumoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
