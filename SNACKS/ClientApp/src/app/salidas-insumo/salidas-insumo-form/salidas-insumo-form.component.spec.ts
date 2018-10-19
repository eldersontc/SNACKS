import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SalidasInsumoFormComponent } from './salidas-insumo-form.component';

describe('SalidasInsumoFormComponent', () => {
  let component: SalidasInsumoFormComponent;
  let fixture: ComponentFixture<SalidasInsumoFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SalidasInsumoFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SalidasInsumoFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
