import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SalidasProductoFormComponent } from './salidas-producto-form.component';

describe('SalidasProductoFormComponent', () => {
  let component: SalidasProductoFormComponent;
  let fixture: ComponentFixture<SalidasProductoFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SalidasProductoFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SalidasProductoFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
