import { Injectable } from '@angular/core';
import { Subject } from '../models/subject.model';

@Injectable({
  providedIn: 'root'
})
export class SubjectService {

  private subjects: Subject[] = [
    { id: 1, name: 'Mathematics' },
    { id: 2, name: 'Physics' }
  ];

  getSubjects(): Subject[] {
    return this.subjects;
  }

  getSubjectById(id: number): Subject | undefined {
    return this.subjects.find(s => s.id === id);
  }

  addSubject(subject: Subject): Subject {
    subject.id = this.subjects.length
      ? Math.max(...this.subjects.map(s => s.id)) + 1
      : 1;

    this.subjects.push(subject);
    return subject;
  }

  updateSubject(id: number, updated: Partial<Subject>): void {
    const index = this.subjects.findIndex(s => s.id === id);

    if (index !== -1) {
      this.subjects[index] = {
        ...this.subjects[index],
        ...updated
      };
    }
  }

  deleteSubject(id: number): void {
    this.subjects = this.subjects.filter(s => s.id !== id);
  }
}