import { Injectable } from "@angular/core";
import { SchoolClass } from "../models/school-class.model";

@Injectable({ providedIn: 'root' })
export class SchoolClassService {

  private classes: SchoolClass[] = [
    { id: 1, name: '6A' },
    { id: 2, name: '6B' }
  ];

  getClasses(): SchoolClass[] {
    return this.classes;
  }

  getById(id: number): SchoolClass | undefined {
    return this.classes.find(c => c.id === id);
  }

  add(cls: SchoolClass): SchoolClass {
    cls.id = this.classes.length
      ? Math.max(...this.classes.map(c => c.id)) + 1
      : 1;

    this.classes.push(cls);
    return cls;
  }

  update(id: number, updated: Partial<SchoolClass>) {
    const index = this.classes.findIndex(c => c.id === id);

    if (index !== -1) {
      this.classes[index] = { ...this.classes[index], ...updated };
    }
  }

  delete(id: number) {
    this.classes = this.classes.filter(c => c.id !== id);
  }
}