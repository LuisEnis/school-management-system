import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { UserService } from '../../../core/services/user.service';
import { User, UserRole } from '../../../core/models/user.model';

@Component({
  selector: 'app-teacher-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './teacher-form.html',
  styleUrl: './teacher-form.css'
})
export class TeacherForm implements OnInit {

  form!: FormGroup;
  teacherId: number | null = null;
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

    this.teacherId = Number(this.route.snapshot.paramMap.get('id'));

    if (this.teacherId) {
      this.isEditMode = true;

      const teacher = this.userService.getUserById(this.teacherId);

      if (teacher) {
        this.form.patchValue(teacher);
      }
    }
  }

  save(): void {

    if (this.isEditMode) {

      this.userService.updateUser(this.teacherId!, this.form.value);

    } else {

      this.userService.addUser({
        role: UserRole.Teacher,
        ...this.form.value
      });

    }

    this.router.navigate(['/teachers']);
  }
}