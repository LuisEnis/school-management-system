import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators
} from '@angular/forms';

import { Router } from '@angular/router';

import { AuthService } from '../../../core/services/auth.service';
import { UserService } from '../../../core/services/user.service';

import { UserDetails } from '../../../core/models/users/user-details.dto';
import { UpdateUserDto } from '../../../core/models/users/update-user.dto';


@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './profile.html',
  styleUrl: './profile.css'
})
export class Profile implements OnInit {


  form!: FormGroup;

  user!: UserDetails;


  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private userService: UserService,
    private router: Router
  ) {}


  ngOnInit(): void {

    const currentUser =
      this.authService.getCurrentUser();


    if (!currentUser) {

      this.router.navigate(['/login']);

      return;

    }


    this.user = currentUser;


    this.form =
      this.fb.group({

        username: [
          currentUser.username,
          Validators.required
        ],

        firstName: [
          currentUser.firstName,
          Validators.required
        ],

        lastName: [
          currentUser.lastName,
          Validators.required
        ],

        email: [
          currentUser.email,
          [
            Validators.required,
            Validators.email
          ]
        ]

      });

  }


  save(): void {

    if (this.form.invalid)
      return;


    const dto: UpdateUserDto = {

      ...this.form.value,

      role: this.user.role

    };


    this.userService
      .update(
        this.user.id,
        dto
      )
      .subscribe({

        next: () => {

          const updatedUser: UserDetails = {

            ...this.user,

            username: dto.username,
            firstName: dto.firstName,
            lastName: dto.lastName,
            email: dto.email

          };


          this.authService
            .updateCurrentUser(updatedUser);


          this.router.navigate(['/dashboard']);

        },

        error: error => {

          console.error(
            'Failed updating profile',
            error
          );

        }

      });

  }

}