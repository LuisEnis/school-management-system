import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { UserService } from '../../../core/services/user.service';
import {UserRole } from '../../../core/models/user.model';
import { SchoolClass } from '../../../core/models/schoolClasses/school-class.model';
import { SchoolClassService } from '../../../core/services/schoolClass.service';
import { AssignmentService } from '../../../core/services/assignment.service';

@Component({
  selector: 'app-student-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './student-form.html',
  styleUrl: './student-form.css'
})
export class StudentForm implements OnInit {

  form!: FormGroup;
  studentId: number | null = null;
  isEditMode = false;
  classes : SchoolClass[] = [];

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService,
    private schoolClassService: SchoolClassService,
    private assignmentService: AssignmentService
  ) {}

  ngOnInit(): void {

    this.form = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: [''],
      classId: [null]
    });

    this.schoolClassService
    .getClasses()
    .subscribe({
        next: classes => {
            this.classes = classes;
        },
        error: error => {
            console.error(
              'Failed loading classes',
              error
            );
        }
    });

    this.studentId = Number(this.route.snapshot.paramMap.get('id'));

    if (this.studentId) {

    this.isEditMode = true;

    this.userService
      .getUserById(this.studentId)
      .subscribe({

        next: student => {

          this.form.patchValue({
            username: student.username,
            firstName: student.firstName,
            lastName: student.lastName,
            email: student.email
          });

        },

        error: error => {

          console.error(
            'Failed loading student',
            error
          );

        }

      });

  }
  }

  save(): void {


    const {
      classId,
      ...studentData
    } = this.form.value;



    if(this.isEditMode){


      this.userService
        .update(
          this.studentId!,
          {
            ...studentData,
            role: UserRole.Student
          }
        )
        .subscribe({

          next: () => {

            this.updateClassAssignment();

          },

          error: error => {

            console.error(
              'Failed updating student',
              error
            );

          }

        });


    }
    else {


      this.userService
        .create({

          ...studentData,

          role: UserRole.Student

        })
        .subscribe({

          next: student => {


            this.assignClass(student.id, classId);


          },

          error: error => {

            console.error(
              'Failed creating student',
              error
            );

          }

        });


    }

  }

  private assignClass(
    studentId:number,
    classId:number | null
  ):void {


    if(!classId){

      this.router.navigate(['/students']);

      return;

    }


    this.assignmentService
      .assignStudentToClass({

        studentId,

        schoolClassId: classId

      })
      .subscribe({

        next: () => {

          this.router.navigate(['/students']);

        },

        error: error => {

          console.error(
            'Failed assigning class',
            error
          );

        }

      });

  }

  private updateClassAssignment():void {


    // For now we simply assign the new class.
    // Later we can load the old assignment and delete it first.

    const classId =
      this.form.value.classId;


    if(classId){

      this.assignmentService
        .assignStudentToClass({

          studentId:this.studentId!,

          schoolClassId:classId

        })
        .subscribe({

          next:()=>{

            this.router.navigate(['/students']);

          }

        });

    }
    else {

      this.router.navigate(['/students']);

    }

  }

}