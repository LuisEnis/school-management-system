import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

import { StudentService } from '../../../core/services/student.service';
import { User, UserRole } from '../../../core/models/user.model';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-teacher-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './teacher-list.html',
  styleUrl: './teacher-list.css'
})
export class TeacherList implements OnInit {

  teachers: User[] = [];

  constructor(private studentService: StudentService) {}

  ngOnInit(): void {
    this.teachers = this.studentService
      .getStudents()
      .filter(u => u.role === UserRole.Teacher);
  }

  delete(id: number) {
    this.studentService.deleteStudent(id);
    this.teachers = this.studentService
      .getStudents()
      .filter(u => u.role === UserRole.Teacher);
  }
}