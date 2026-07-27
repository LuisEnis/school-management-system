import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { Router } from '@angular/router';

import { UserService } from '../../../core/services/user.service';
import { ChangePasswordDto } from '../../../core/models/users/change-password.dto';


@Component({
  selector: 'app-change-password',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './change-password.html',
  styleUrl: './change-password.css'
})
export class ChangePassword {


  form!: FormGroup;

  successMessage = '';
  errorMessage = '';



  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private router: Router
  ) {


    this.form = this.fb.group({

      currentPassword: [
        '',
        Validators.required
      ],


      newPassword: [
        '',
        [
          Validators.required,
          Validators.minLength(6)
        ]
      ],


      confirmNewPassword: [
        '',
        Validators.required
      ]

    });


  }




  save(): void {


    this.successMessage = '';
    this.errorMessage = '';



    if(this.form.invalid)
      return;



    const dto: ChangePasswordDto =
      this.form.value;



    if(
      dto.newPassword !== 
      dto.confirmNewPassword
    ){

      this.errorMessage =
        'New passwords do not match.';

      return;

    }




    this.userService
      .changePassword(dto)
      .subscribe({

        next:()=>{

          this.successMessage =
            'Password changed successfully.';


          this.form.reset();


        },


        error:()=>{

          this.errorMessage =
            'Failed changing password.';

        }

      });


  }



}