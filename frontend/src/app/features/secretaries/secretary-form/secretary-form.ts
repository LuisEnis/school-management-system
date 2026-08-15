import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators
} from '@angular/forms';

import {
  ActivatedRoute,
  Router
} from '@angular/router';

import { UserService } from '../../../core/services/user.service';
import { UserRole } from '../../../core/models/user.model';

import { UpdateUserDto }
from '../../../core/models/users/update-user.dto';

import { CreateUserDto }
from '../../../core/models/users/create-user.dto';


@Component({
  selector: 'app-secretary-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './secretary-form.html',
  styleUrl: './secretary-form.css'
})
export class SecretaryForm implements OnInit {


  form!: FormGroup;

  secretaryId: number | null = null;

  isEditMode = false;


  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService
  ) {}


  ngOnInit(): void {

    this.form = this.fb.group({

      username: [
        '',
        Validators.required
      ],

      firstName: [
        '',
        Validators.required
      ],

      lastName: [
        '',
        Validators.required
      ],

      email: [
        '',
        [
          Validators.required,
          Validators.email
        ]
      ],

      password: [
        ''
      ]

    });


    this.secretaryId =
      Number(
        this.route.snapshot.paramMap.get('id')
      );


    if (this.secretaryId) {

      this.isEditMode = true;


      this.userService
        .getUserById(this.secretaryId)
        .subscribe({

          next: secretary => {

            this.form.patchValue(secretary);

          }

        });

    }

  }


  save(): void {

    if (this.isEditMode) {

      const dto: UpdateUserDto = {

        ...this.form.value,

        role: UserRole.Secretary

      };


      this.userService
        .update(
          this.secretaryId!,
          dto
        )
        .subscribe({

          next: () => {

            this.router.navigate(['/secretaries']);

          }

        });

    }
    else {

      const dto: CreateUserDto = {

        ...this.form.value,

        role: UserRole.Secretary

      };


      this.userService
        .create(dto)
        .subscribe({

          next: () => {

            this.router.navigate(['/secretaries']);

          }

        });

    }

  }

}