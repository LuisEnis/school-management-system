import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SchoolClassForm } from './school-class-form';

describe('SchoolClassForm', () => {
  let component: SchoolClassForm;
  let fixture: ComponentFixture<SchoolClassForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SchoolClassForm],
    }).compileComponents();

    fixture = TestBed.createComponent(SchoolClassForm);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
