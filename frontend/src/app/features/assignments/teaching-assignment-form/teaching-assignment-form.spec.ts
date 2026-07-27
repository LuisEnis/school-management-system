import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TeachingAssignmentForm } from './teaching-assignment-form';

describe('TeachingAssignmentForm', () => {
  let component: TeachingAssignmentForm;
  let fixture: ComponentFixture<TeachingAssignmentForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TeachingAssignmentForm],
    }).compileComponents();

    fixture = TestBed.createComponent(TeachingAssignmentForm);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
