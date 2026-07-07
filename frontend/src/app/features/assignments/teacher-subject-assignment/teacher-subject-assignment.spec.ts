import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TeacherSubjectAssignment } from './teacher-subject-assignment';

describe('TeacherSubjectAssignment', () => {
  let component: TeacherSubjectAssignment;
  let fixture: ComponentFixture<TeacherSubjectAssignment>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TeacherSubjectAssignment],
    }).compileComponents();

    fixture = TestBed.createComponent(TeacherSubjectAssignment);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
