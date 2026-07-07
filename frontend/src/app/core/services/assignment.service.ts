import { Injectable } from "@angular/core";
import { StudentClass } from "../models/student-class.model";
import { SchoolClass } from "../models/school-class.model";
import { TeacherSubject } from "../models/teacher-subject.model";
import { Subject } from "../models/subject.model";
import { TeachingAssignment } from "../models/teaching-assignment.model";

@Injectable({
  providedIn: 'root'
})

export class AssignmentService {
private studentClasses: StudentClass[] = [
    {
      id: 1,
      studentId: 1,
      classId: 1
    },
    {
      id: 2,
      studentId: 2,
      classId: 2
    }
  ];


  private teacherSubjects: TeacherSubject[] = [
    {
      id: 1,
      teacherId: 3,
      subjectId: 1
    },
    {
      id: 2,
      teacherId: 4,
      subjectId: 2
    }
  ];


  private teachingAssignments: TeachingAssignment[] = [
    {
      id: 1,
      schoolClassId: 1,
      subjectId: 1,
      teacherId: 3
    },
    {
      id: 2,
      schoolClassId: 1,
      subjectId: 2,
      teacherId: 3
    },
    {
      id: 3,
      schoolClassId: 2,
      subjectId: 1,
      teacherId: 4
    }
  ];


  // ============================
  // Student - Class
  // ============================

  getStudentClasses(): StudentClass[] {
    return this.studentClasses;
  }


  getStudentClass(studentId: number): StudentClass | undefined {

    return this.studentClasses.find(
      sc => sc.studentId === studentId
    );

  }


  assignStudentToClass(
    studentId: number,
    classId: number | null
  ): void {


    this.studentClasses =
      this.studentClasses.filter(
        sc => sc.studentId !== studentId
      );


    if (classId) {

      const newId = this.studentClasses.length
        ? Math.max(...this.studentClasses.map(sc => sc.id)) + 1
        : 1;


      this.studentClasses.push({
        id: newId,
        studentId,
        classId
      });

    }
  }



  // ============================
  // Teacher - Subject
  // ============================

  getTeacherSubjects(): TeacherSubject[] {

    return this.teacherSubjects;

  }


  assignTeacherToSubject(
    teacherId: number,
    subjectId: number
  ): void {


    const exists = this.teacherSubjects.some(
      ts =>
        ts.teacherId === teacherId &&
        ts.subjectId === subjectId
    );


    if (!exists) {

      const newId = this.teacherSubjects.length
        ? Math.max(...this.teacherSubjects.map(ts => ts.id)) + 1
        : 1;


      this.teacherSubjects.push({
        id: newId,
        teacherId,
        subjectId
      });

    }

  }


  deleteTeacherSubject(
    teacherId: number,
    subjectId: number
  ): void {


    this.teacherSubjects =
      this.teacherSubjects.filter(
        ts =>
          !(
            ts.teacherId === teacherId &&
            ts.subjectId === subjectId
          )
      );

  }



  // ============================
  // Teaching Assignment
  // ============================

  getTeachingAssignments(): TeachingAssignment[] {

    return this.teachingAssignments;

  }


  addTeachingAssignment(
    schoolClassId: number,
    subjectId: number,
    teacherId: number
  ): void {


    const exists =
      this.teachingAssignments.some(
        ta =>
          ta.schoolClassId === schoolClassId &&
          ta.subjectId === subjectId &&
          ta.teacherId === teacherId
      );


    if (!exists) {

      const newId = this.teachingAssignments.length
        ? Math.max(...this.teachingAssignments.map(ta => ta.id)) + 1
        : 1;


      this.teachingAssignments.push({
        id: newId,
        schoolClassId,
        subjectId,
        teacherId
      });

    }

  }



  deleteTeachingAssignment(id: number): void {

    this.teachingAssignments =
      this.teachingAssignments.filter(
        ta => ta.id !== id
      );

  }



  getAssignmentsForTeacher(
    teacherId: number
  ): TeachingAssignment[] {


    return this.teachingAssignments.filter(
      ta => ta.teacherId === teacherId
    );

  }



  getAssignmentsForClass(
    schoolClassId: number
  ): TeachingAssignment[] {


    return this.teachingAssignments.filter(
      ta => ta.schoolClassId === schoolClassId
    );

  }



  getAssignmentsForStudentClass(
    schoolClassId: number
  ): TeachingAssignment[] {


    return this.teachingAssignments.filter(
      ta => ta.schoolClassId === schoolClassId
    );

  }

}