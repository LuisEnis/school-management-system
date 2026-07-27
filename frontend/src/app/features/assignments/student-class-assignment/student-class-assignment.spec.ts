import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudentClassAssignment } from './student-class-assignment';

describe('StudentClassAssignment', () => {
  let component: StudentClassAssignment;
  let fixture: ComponentFixture<StudentClassAssignment>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StudentClassAssignment],
    }).compileComponents();

    fixture = TestBed.createComponent(StudentClassAssignment);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
