import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UserService } from '../../../core/services/user.service';
import { User, UserRole } from '../../../core/models/user.model';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AssignmentService } from '../../../core/services/assignment.service';
import { SchoolClassService } from '../../../core/services/schoolClass.service';

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

  constructor(
    private userService: UserService,
    private assignmentService: AssignmentService,
    private schoolClassService: SchoolClassService
  ) {}

  ngOnInit(): void {
    this.students = this.userService.getUsersByRole(UserRole.Student);
  }

  getClassName(studentId: number): string {

    const studentClass =
      this.assignmentService.getStudentClass(studentId);

    if (!studentClass) {
      return 'Not Assigned';
    }


    const schoolClass =
      this.schoolClassService.getById(studentClass.classId);


    return schoolClass?.name || 'Not Assigned';
  }

  delete(id: number) {
    this.userService.deleteUser(id);
    this.students = this.userService.getUsersByRole(UserRole.Student);
  }
}