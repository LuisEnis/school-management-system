import { Injectable } from '@angular/core';
import { User, UserRole } from '../models/user.model';
import { SchoolClass } from '../models/school-class.model';
import { StudentClass } from '../models/student-class.model';

@Injectable({
  providedIn: 'root'
})
export class StudentService {

  private students: User[] = [
    {
      id: 1,
      username: 'john',
      password: '123',
      firstName: 'John',
      lastName: 'Doe',
      email: 'john@test.com',
      role: UserRole.Student
    },
    {
      id: 2,
      username: 'anna',
      password: '123',
      firstName: 'Anna',
      lastName: 'Smith',
      email: 'anna@test.com',
      role: UserRole.Student
    },
    {
      id: 3,
      username: 'luis',
      password: '123',
      firstName: 'Luis',
      lastName: 'Teacher',
      email: 'luis@teacher.com',
      role: UserRole.Teacher
    },
    {
      id: 2,
      username: 'brandon',
      password: '123',
      firstName: 'Brandon',
      lastName: 'Teacher',
      email: 'brandon@teacher.com',
      role: UserRole.Teacher
    }
  ];

  private classes: SchoolClass[] = [
    { id: 1, name: '6A' },
    { id: 2, name: '6B' }
  ];

  private studentClasses: StudentClass[] = [
    { studentId: 1, classId: 1 },
    { studentId: 2, classId: 2 }
  ];

  getStudents(): User[] {
    return this.students;
  }

  getClasses(): SchoolClass[] {
    return this.classes;
  }

  getStudentClass(studentId: number): SchoolClass | null {
  const relation = this.studentClasses.find(sc => sc.studentId === studentId);

  if (!relation) return null;

  return this.classes.find(c => c.id === relation.classId) || null;
}

  getStudentById(id: number): User | undefined {
    return this.students.find(s => s.id === id);
  }

  addStudent(student: User): User {
    student.id = Math.max(...this.students.map(s => s.id)) + 1;
    this.students.push(student);
    return student;
  }

  updateStudent(id: number, updated: Partial<User>): void {
    const index = this.students.findIndex(s => s.id === id);

    if (index !== -1) {
        this.students[index] = {
        ...this.students[index],
        ...updated
        };
    }
  }

  deleteStudent(id: number): void {
    this.students = this.students.filter(s => s.id !== id);
  }

  assignStudentToClass(studentId: number, classId: number | null) {

    // remove old relation first
    this.studentClasses = this.studentClasses.filter(sc => sc.studentId !== studentId);

    // add new relation if class selected
    if (classId) {
        this.studentClasses.push({
        studentId,
        classId
        });
    }
  }
}