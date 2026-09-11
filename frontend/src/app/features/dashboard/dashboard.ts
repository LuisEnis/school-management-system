import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable, startWith, Subject, Subscription, switchMap } from 'rxjs';

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
import { SignalRService } from '../../core/services/signalr.service';



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
export class Dashboard implements OnInit, OnDestroy {

  private reload$ = new Subject<void>();
  private subscriptions = new Subscription();


  UserRole = UserRole;


  studentDashboard$!: Observable<StudentDashboardDto>;

  teacherAssignments$!: Observable<TeacherAssignmentDto[]>;

  managementDashboard$!: Observable<ManagementDashboardDto>;


  constructor(
    private authService: AuthService,
    private studentService: StudentService,
    private teacherService: TeacherService,
    private dashboardService: DashboardService,
    private signalRService: SignalRService
  ){}



  get user(): UserDetails | null {

    return this.authService.getCurrentUser();

  }



  ngOnInit(): void {
    this.signalRService.startConnection();


    if (
      this.user?.role === UserRole.Director ||
      this.user?.role === UserRole.Secretary
    ) {

      this.managementDashboard$ =
        this.reload$.pipe(
          startWith(null),
          switchMap(() =>
            this.dashboardService.getManagementDashboard()
          )
        );

        this.subscriptions.add(
          this.signalRService.studentCreated$
            .subscribe(() => this.reload$.next())
        );

        this.subscriptions.add(
          this.signalRService.studentDeleted$
            .subscribe(() => this.reload$.next())
        );

        this.subscriptions.add(
          this.signalRService.teacherCreated$
            .subscribe(() => this.reload$.next())
        );

        this.subscriptions.add(
          this.signalRService.teacherDeleted$
            .subscribe(() => this.reload$.next())
        );

        this.subscriptions.add(
          this.signalRService.subjectCreated$
            .subscribe(() => this.reload$.next())
        );

        this.subscriptions.add(
          this.signalRService.subjectDeleted$
            .subscribe(() => this.reload$.next())
        );

        this.subscriptions.add(
          this.signalRService.classCreated$
            .subscribe(() => this.reload$.next())
        );

        this.subscriptions.add(
          this.signalRService.classDeleted$
            .subscribe(() => this.reload$.next())
        );

        this.subscriptions.add(
          this.signalRService.studentClassAssigned$
            .subscribe(() => this.reload$.next())
        );

        this.subscriptions.add(
          this.signalRService.studentClassRemoved$
            .subscribe(() => this.reload$.next())
        );

        this.subscriptions.add(
          this.signalRService.teacherSubjectAssigned$
            .subscribe(() => this.reload$.next())
        );

        this.subscriptions.add(
          this.signalRService.teacherSubjectRemoved$
            .subscribe(() => this.reload$.next())
        );

        this.subscriptions.add(
          this.signalRService.teachingAssignmentCreated$
            .subscribe(() => this.reload$.next())
        );

        this.subscriptions.add(
          this.signalRService.teachingAssignmentRemoved$
            .subscribe(() => this.reload$.next())
        );

    }
    
    
    if(this.user?.role === UserRole.Student){

      this.studentDashboard$ =
        this.reload$.pipe(
          startWith(null),
          switchMap(() =>
            this.studentService.getDashboard()
          )
        );

        this.subscriptions.add(
          this.signalRService.studentClassAssigned$
            .subscribe(() => this.reload$.next())
        );

        this.subscriptions.add(
          this.signalRService.studentClassRemoved$
            .subscribe(() => this.reload$.next())
        );

        this.subscriptions.add(
          this.signalRService.teachingAssignmentCreated$
            .subscribe(() => this.reload$.next())
        );

        this.subscriptions.add(
          this.signalRService.teachingAssignmentRemoved$
            .subscribe(() => this.reload$.next())
        );

    }



    if(this.user?.role === UserRole.Teacher){

      this.teacherAssignments$ =
        this.reload$.pipe(
          startWith(null),
          switchMap(() =>
            this.teacherService.getClasses()
          )
        );

        this.subscriptions.add(
          this.signalRService.teacherSubjectAssigned$
            .subscribe(() => this.reload$.next())
        );

        this.subscriptions.add(
          this.signalRService.teacherSubjectRemoved$
            .subscribe(() => this.reload$.next())
        );

        this.subscriptions.add(
          this.signalRService.teachingAssignmentCreated$
            .subscribe(() => this.reload$.next())
        );

        this.subscriptions.add(
          this.signalRService.teachingAssignmentRemoved$
            .subscribe(() => this.reload$.next())
        );

    }


  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }


}