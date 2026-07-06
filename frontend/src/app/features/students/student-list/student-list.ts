import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

import { StudentService } from '../../../core/services/student.service';
import { User, UserRole } from '../../../core/models/user.model';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-student-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './student-list.html',
  styleUrl: './student-list.css'
})
export class StudentList implements OnInit {

  students: User[] = [];

  showForm = false;

  newStudent = {
    firstName: '',
    lastName: '',
    username: '',
    email: '',
    password: ''
  };

  constructor(private studentService: StudentService) {}

  ngOnInit(): void {
    this.students = this.studentService.getStudents().filter(u => u.role === UserRole.Teacher);;
  }

  getClassName(studentId: number): string {
    return this.studentService.getStudentClass(studentId)?.name || 'Not Assigned';
  }

  delete(id: number) {
    this.studentService.deleteStudent(id);
    this.students = this.studentService.getStudents().filter(u => u.role === UserRole.Teacher);;
  }
}