import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

import { CreateStudentClassAssignmentDto } 
from '../models/assignments/create-student-class-assignment.dto';

import { CreateTeacherSubjectAssignmentDto }
from '../models/assignments/create-teacher-subject-assignment.dto';

import { CreateTeachingAssignmentDto }
from '../models/assignments/create-teaching-assignment.dto';
import { StudentClass } from '../models/assignments/student-class.model';
import { TeacherSubject } from '../models/assignments/teacher-subject.model';
import { TeachingAssignment } from '../models/assignments/teaching-assignment.model';
import { TeachingAssignmentDto } from '../models/assignments/teaching-assignment.dto';
import { TeacherSubjectAssignmentDto } from '../models/assignments/teacher-subject-assignment.dto';
import { StudentClassAssignmentDto } from '../models/assignments/student-class-assignment.dto';


@Injectable({
  providedIn:'root'
})
export class AssignmentService {


  private apiUrl =
  `${environment.apiUrl}/assignments`;


  constructor(
    private http:HttpClient
  ){}


  getStudentClassAssignments(): Observable<StudentClassAssignmentDto[]> {

    return this.http.get<StudentClassAssignmentDto[]>(
      `${this.apiUrl}/student-class`
    );

  }



  getTeacherSubjectAssignments(): Observable<TeacherSubjectAssignmentDto[]> {

    return this.http.get<TeacherSubjectAssignmentDto[]>(
      `${this.apiUrl}/teacher-subject`
    );

  }



  getTeachingAssignments(): Observable<TeachingAssignmentDto[]> {

    return this.http.get<TeachingAssignmentDto[]>(
      `${this.apiUrl}/teaching-assignment`
    );

  }


  assignStudentToClass(
    dto:CreateStudentClassAssignmentDto
  ):Observable<Observable<StudentClass>>{

    return this.http.post<Observable<StudentClass>>(
      `${this.apiUrl}/student-class`,
      dto
    );

  }



  removeStudentFromClass(
    studentId:number,
    schoolClassId:number
  ):Observable<void>{

    return this.http.delete<void>(
      `${this.apiUrl}/student-class`,
      {
        params:{
          studentId,
          schoolClassId
        }
      }
    );

  }



  assignTeacherToSubject(
    dto:CreateTeacherSubjectAssignmentDto
  ):Observable<TeacherSubject>{

    return this.http.post<TeacherSubject>(
      `${this.apiUrl}/teacher-subject`,
      dto
    );

  }



  removeTeacherFromSubject(
    teacherId:number,
    subjectId:number
  ):Observable<void>{

    return this.http.delete<void>(
      `${this.apiUrl}/teacher-subject`,
      {
        params:{
          teacherId,
          subjectId
        }
      }
    );

  }



  assignTeachingAssignment(
    dto:CreateTeachingAssignmentDto
  ):Observable<TeachingAssignment>{

    return this.http.post<TeachingAssignment>(
      `${this.apiUrl}/teaching-assignment`,
      dto
    );

  }



  removeTeachingAssignment(
    schoolClassId:number,
    subjectId:number,
    teacherId:number
  ):Observable<void>{

    return this.http.delete<void>(
      `${this.apiUrl}/teaching-assignment`,
      {
        params:{
          schoolClassId,
          subjectId,
          teacherId
        }
      }
    );

  }

}