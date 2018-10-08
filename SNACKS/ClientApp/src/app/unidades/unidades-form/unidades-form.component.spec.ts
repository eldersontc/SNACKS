import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UnidadesFormComponent } from './unidades-form.component';

describe('UnidadesFormComponent', () => {
  let component: UnidadesFormComponent;
  let fixture: ComponentFixture<UnidadesFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UnidadesFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UnidadesFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
