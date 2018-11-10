import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CajasFormComponent } from './cajas-form.component';

describe('CajasFormComponent', () => {
  let component: CajasFormComponent;
  let fixture: ComponentFixture<CajasFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CajasFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CajasFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
