import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TeacherSubjectAssignmentForm } from './teacher-subject-assignment-form';

describe('TeacherSubjectAssignmentForm', () => {
  let component: TeacherSubjectAssignmentForm;
  let fixture: ComponentFixture<TeacherSubjectAssignmentForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TeacherSubjectAssignmentForm],
    }).compileComponents();

    fixture = TestBed.createComponent(TeacherSubjectAssignmentForm);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
