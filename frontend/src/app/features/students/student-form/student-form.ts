import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { UserService } from '../../../core/services/user.service';
import {UserRole } from '../../../core/models/user.model';

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

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService
  ) {}

  ngOnInit(): void {

    this.form = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['']
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

            this.router.navigate(['/students']);

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


            this.router.navigate(['/students']);


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

}