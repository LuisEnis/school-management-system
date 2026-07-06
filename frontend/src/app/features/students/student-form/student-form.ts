import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { StudentService } from '../../../core/services/student.service';
import { User } from '../../../core/models/user.model';
import { SchoolClass } from '../../../core/models/school-class.model';

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
    private studentService: StudentService
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

    this.classes = this.studentService.getClasses();

    this.studentId = Number(this.route.snapshot.paramMap.get('id'));

    if (this.studentId) {
      this.isEditMode = true;
      const student = this.studentService.getStudentById(this.studentId);

      if (student) {

        const studentClass = this.studentService.getStudentClass(student.id);

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

      this.studentService.updateStudent(this.studentId!, studentData);

      this.studentService.assignStudentToClass(this.studentId!, classId);

    } else {

      const newStudent = this.studentService.addStudent({
        id: 0,
        role: 'Student',
        ...studentData
      });

      this.studentService.assignStudentToClass(newStudent.id, classId);
    }

    this.router.navigate(['/students']);
  }
}