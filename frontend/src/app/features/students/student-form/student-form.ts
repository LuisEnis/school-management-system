import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { UserService } from '../../../core/services/user.service';
import { User, UserRole } from '../../../core/models/user.model';
import { SchoolClass } from '../../../core/models/school-class.model';
import { SchoolClassService } from '../../../core/services/schoolClass.service';
import { AssignmentService } from '../../../core/services/assignment.service';

@Component({
  selector: 'app-student-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './student-form.html',
  styleUrl: './student-form.css'
})
export class StudentForm implements OnInit {

  form!: FormGroup;
  studentId: number | null = null;
  isEditMode = false;
  classes : SchoolClass[] = [];

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService,
    private schoolClassService: SchoolClassService,
    private assignmentService: AssignmentService
  ) {}

  ngOnInit(): void {

    this.form = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: [''],
      classId: [null]
    });

    this.classes = this.schoolClassService.getClasses();

    this.studentId = Number(this.route.snapshot.paramMap.get('id'));

    if (this.studentId) {
      this.isEditMode = true;
      const student = this.userService.getUserById(this.studentId);

      if (student) {

        const studentClass = this.assignmentService.getStudentClass(student.id);

        this.form.patchValue({
          ...student,
          classId: studentClass ? studentClass.id : null
        });
      }
    }
  }

  save(): void {

    const { classId, ...studentData } = this.form.value;

    if (this.isEditMode) {

      this.userService.updateUser(this.studentId!, studentData);

      this.assignmentService.assignStudentToClass(this.studentId!, classId);

    } else {

      const newStudent = this.userService.addUser({
        role: UserRole.Student,
        ...studentData
      });

      this.assignmentService.assignStudentToClass(newStudent.id, classId);
    }

    this.router.navigate(['/students']);
  }
}