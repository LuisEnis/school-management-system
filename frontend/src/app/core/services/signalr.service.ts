import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';

import { environment } from '../../../environments/environment';
import { UserDto } from '../models/users/user.dto';
import { Subject as SubjectModel } from '../models/subjects/subject.model';
import { SchoolClass } from '../models/schoolClasses/school-class.model';
import { StudentClassAssignmentDto } from '../models/assignments/student-class-assignment.dto';
import { TeacherSubjectAssignmentDto } from '../models/assignments/teacher-subject-assignment.dto';
import { TeachingAssignmentDto } from '../models/assignments/teaching-assignment.dto';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {

  private hubConnection?: signalR.HubConnection;

  private studentCreatedSubject = new Subject<UserDto>();
  private studentUpdatedSubject = new Subject<UserDto>();
  private studentDeletedSubject = new Subject<number>();
  private teacherCreatedSubject = new Subject<UserDto>();
  private teacherUpdatedSubject = new Subject<UserDto>();
  private teacherDeletedSubject = new Subject<number>();
  private subjectCreatedSubject = new Subject<SubjectModel>();
  private subjectUpdatedSubject = new Subject<SubjectModel>();
  private subjectDeletedSubject = new Subject<number>();
  private classCreatedSubject = new Subject<SchoolClass>();
  private classUpdatedSubject = new Subject<SchoolClass>();
  private classDeletedSubject = new Subject<number>();
  private studentClassAssignedSubject = new Subject<StudentClassAssignmentDto>();
  private studentClassRemovedSubject = new Subject<void>();
  private teacherSubjectAssignedSubject = new Subject<TeacherSubjectAssignmentDto>();
  private teacherSubjectRemovedSubject = new Subject<void>();
  private teachingAssignmentCreatedSubject = new Subject<TeachingAssignmentDto>();
  private teachingAssignmentRemovedSubject = new Subject<void>();


  studentCreated$ = this.studentCreatedSubject.asObservable();
  studentUpdated$ = this.studentUpdatedSubject.asObservable();
  studentDeleted$ = this.studentDeletedSubject.asObservable();
  teacherCreated$ = this.teacherCreatedSubject.asObservable();
  teacherUpdated$ = this.teacherUpdatedSubject.asObservable();
  teacherDeleted$ = this.teacherDeletedSubject.asObservable();
  subjectCreated$ = this.subjectCreatedSubject.asObservable();
  subjectUpdated$ = this.subjectUpdatedSubject.asObservable();
  subjectDeleted$ = this.subjectDeletedSubject.asObservable();
  classCreated$ = this.classCreatedSubject.asObservable();
  classUpdated$ = this.classUpdatedSubject.asObservable();
  classDeleted$ = this.classDeletedSubject.asObservable();
  studentClassAssigned$ = this.studentClassAssignedSubject.asObservable();
  studentClassRemoved$ = this.studentClassRemovedSubject.asObservable();
  teacherSubjectAssigned$ = this.teacherSubjectAssignedSubject.asObservable();
  teacherSubjectRemoved$ = this.teacherSubjectRemovedSubject.asObservable();
  teachingAssignmentCreated$ = this.teachingAssignmentCreatedSubject.asObservable();
  teachingAssignmentRemoved$ = this.teachingAssignmentRemovedSubject.asObservable();

  constructor(
    private authService: AuthService
  ) {}

  startConnection(): void {
    if (this.hubConnection) {
      return;
    }

    const hubUrl =
      environment.production
        ? '/hubs/school'
        : 'https://localhost:7233/hubs/school';

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(hubUrl, {
        accessTokenFactory: () => {
        return this.authService.getToken() ?? '';
        }
      })
      .withAutomaticReconnect()
      .build();

    this.registerListeners();

    this.hubConnection
      .start()
      .then(() => {
        console.log('SignalR connected');
      })
      .catch(error => {
        console.error('SignalR connection error:', error);
      });
  }

  stopConnection(): void {
    if (!this.hubConnection) {
      return;
    }

    this.hubConnection
      .stop()
      .catch(error => {
        console.error('SignalR disconnection error:', error);
      });

    this.hubConnection = undefined;
  }

  private registerListeners(): void {
    if (!this.hubConnection) {
      return;
    }

    this.hubConnection.on(
      'StudentCreated',
      (student: UserDto) => {
        this.studentCreatedSubject.next(student);
      }
    );

    this.hubConnection.on(
      'StudentUpdated',
      (student: UserDto) => {
        this.studentUpdatedSubject.next(student);
      }
    );

    this.hubConnection.on(
      'StudentDeleted',
      (studentId: number) => {
        this.studentDeletedSubject.next(studentId);
      }
    );

    this.hubConnection.on(
        'TeacherCreated',
        (teacher: UserDto) => {
            this.teacherCreatedSubject.next(teacher);
        }
    );

    this.hubConnection.on(
        'TeacherUpdated',
        (teacher: UserDto) => {
            this.teacherUpdatedSubject.next(teacher);
        }
    );

    this.hubConnection.on(
        'TeacherDeleted',
        (teacherId: number) => {
            this.teacherDeletedSubject.next(teacherId);
        }
    );

    this.hubConnection.on(
        'SubjectCreated',
        (subject: SubjectModel) => {
            this.subjectCreatedSubject.next(subject);
        }
    );

    this.hubConnection.on(
        'SubjectUpdated',
        (subject: SubjectModel) => {
            this.subjectUpdatedSubject.next(subject);
        }
    );

    this.hubConnection.on(
        'SubjectDeleted',
        (subjectId: number) => {
            this.subjectDeletedSubject.next(subjectId);
        }
    );

    this.hubConnection.on(
        'ClassCreated',
        (schoolClass: SchoolClass) => {
            this.classCreatedSubject.next(schoolClass);
        }
    );

    this.hubConnection.on(
        'ClassUpdated',
        (schoolClass: SchoolClass) => {
            this.classUpdatedSubject.next(schoolClass);
        }
    );

    this.hubConnection.on(
        'ClassDeleted',
        (classId: number) => {
            this.classDeletedSubject.next(classId);
        }
    );

    this.hubConnection.on(
        'StudentClassAssigned',
        (assignment: StudentClassAssignmentDto) => {
            this.studentClassAssignedSubject.next(assignment);
        }
    );

    this.hubConnection.on(
        'StudentClassRemoved',
        () => {
            this.studentClassRemovedSubject.next();
        }
    );

    this.hubConnection.on(
        'TeacherSubjectAssigned',
        (assignment: TeacherSubjectAssignmentDto) => {
            this.teacherSubjectAssignedSubject.next(assignment);
        }
    );

    this.hubConnection.on(
        'TeacherSubjectRemoved',
        () => {
            this.teacherSubjectRemovedSubject.next();
        }
    );

    this.hubConnection.on(
        'TeachingAssignmentCreated',
        (assignment: TeachingAssignmentDto) => {
            this.teachingAssignmentCreatedSubject.next(assignment);
        }
    );

    this.hubConnection.on(
        'TeachingAssignmentRemoved',
        () => {
            this.teachingAssignmentRemovedSubject.next();
        }
    );

  }
}