import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UserService } from '../../../core/services/user.service';
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

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.teachers = this.userService
      .getUsersByRole(UserRole.Teacher);
  }

  delete(id: number) {
    this.userService.deleteUser(id);
    this.teachers = this.userService
      .getUsersByRole(UserRole.Teacher);
  }
}