import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';

import { AuthService } from '../../core/services/auth.service';
import { StudentService } from '../../core/services/student.service';
import { TeacherService } from '../../core/services/teacher.service';

import { UserDetails } from '../../core/models/users/user-details.dto';
import { UserRole } from '../../core/models/user.model';

import { StudentDashboardDto } 
from '../../core/models/students/student-dashboard.dto';

import { TeacherAssignmentDto } 
from '../../core/models/teachers/teacher-assignment.dto';
import { RouterLink } from '@angular/router';
import { ManagementDashboardDto } from '../../core/models/managementDashboard/management-dashboard.dto';
import { DashboardService } from '../../core/services/dashboard.service';



@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink
  ],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard implements OnInit {


  UserRole = UserRole;


  studentDashboard$!: Observable<StudentDashboardDto>;

  teacherAssignments$!: Observable<TeacherAssignmentDto[]>;

  managementDashboard$!: Observable<ManagementDashboardDto>;


  constructor(
    private authService: AuthService,
    private studentService: StudentService,
    private teacherService: TeacherService,
    private dashboardService: DashboardService
  ){}



  get user(): UserDetails | null {

    return this.authService.getCurrentUser();

  }



  ngOnInit(): void {


    if (
      this.user?.role === UserRole.Director ||
      this.user?.role === UserRole.Secretary
    ) {

      this.managementDashboard$ =
        this.dashboardService
          .getManagementDashboard();

    }
    
    
    if(this.user?.role === UserRole.Student){

      this.studentDashboard$ =
        this.studentService.getDashboard();

    }



    if(this.user?.role === UserRole.Teacher){

      this.teacherAssignments$ =
        this.teacherService.getClasses();

    }


  }


}