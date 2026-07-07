import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import { UserService } from '../../../core/services/user.service';
import { User, UserRole } from '../../../core/models/user.model';
import { Subject } from '../../../core/models/subject.model';
import { TeacherSubject } from '../../../core/models/teacher-subject.model';
import { AssignmentService } from '../../../core/services/assignment.service';
import { SubjectService } from '../../../core/services/subject.service';

@Component({
  selector: 'app-teacher-subject-assignment',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './teacher-subject-assignment.html',
  styleUrl: './teacher-subject-assignment.css'
})
export class TeacherSubjectAssignment implements OnInit {

  // Form for creating assignments
  form!: FormGroup;

  // Separate form for filtering table
  filterForm!: FormGroup;


  teachers: User[] = [];
  subjects: Subject[] = [];


  assignments: TeacherSubject[] = [];
  filteredAssignments: TeacherSubject[] = [];


  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private subjectService: SubjectService,
    private assignmentService: AssignmentService
  ) {}


  ngOnInit(): void {


    // Assignment form
    this.form = this.fb.group({

      teacherId: [null, Validators.required],
      subjectId: [null, Validators.required]

    });



    // Filter form
    this.filterForm = this.fb.group({

      teacherId: [null],
      subjectId: [null]

    });



    // Load teachers
    this.teachers = this.userService
      .getUsersByRole(UserRole.Teacher);



    // Load subjects
    this.subjects = this.subjectService.getSubjects();



    // Load existing assignments
    this.loadAssignments();



    // Update table whenever filters change
    this.filterForm.valueChanges.subscribe(() => {

      this.applyFilters();

    });

  }



  save(): void {


    const {
      teacherId,
      subjectId

    } = this.form.value;



    this.assignmentService.assignTeacherToSubject(

      Number(teacherId),
      Number(subjectId)

    );



    this.loadAssignments();



    // Keep teacher selected for easier multiple assignments
    // Clear only subject

    this.form.patchValue({

      subjectId: null

    });

  }




  loadAssignments(): void {


    this.assignments =
      this.assignmentService.getTeacherSubjects();



    this.applyFilters();

  }




  applyFilters(): void {


    const {
      teacherId,
      subjectId

    } = this.filterForm.value;



    this.filteredAssignments =
      this.assignments.filter(assignment => {



        const teacherMatches =
          !teacherId ||
          assignment.teacherId === Number(teacherId);



        const subjectMatches =
          !subjectId ||
          assignment.subjectId === Number(subjectId);



        return teacherMatches && subjectMatches;

      });

  }




  deleteAssignment(
    teacherId: number,
    subjectId: number
  ): void {


    this.assignmentService.deleteTeacherSubject(

      teacherId,
      subjectId

    );


    this.loadAssignments();

  }




  getTeacherName(id: number): string {


    const teacher =
      this.teachers.find(t => t.id === id);



    if (!teacher) {
      return 'Unknown';
    }



    return `${teacher.firstName} ${teacher.lastName}`;

  }




  getSubjectName(id: number): string {


    const subject =
      this.subjects.find(s => s.id === id);



    return subject
      ? subject.name
      : 'Unknown';

  }

}