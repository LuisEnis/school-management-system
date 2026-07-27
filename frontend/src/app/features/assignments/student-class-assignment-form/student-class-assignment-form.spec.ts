import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudentClassAssignmentForm } from './student-class-assignment-form';

describe('StudentClassAssignmentForm', () => {
  let component: StudentClassAssignmentForm;
  let fixture: ComponentFixture<StudentClassAssignmentForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StudentClassAssignmentForm],
    }).compileComponents();

    fixture = TestBed.createComponent(StudentClassAssignmentForm);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
