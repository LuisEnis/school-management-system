import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { UserService } from '../../../core/services/user.service';
import { UserDto } from '../../../core/models/users/user.dto';
import { Observable } from 'rxjs';


@Component({
  selector: 'app-student-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink
  ],
  templateUrl: './student-list.html',
  styleUrl: './student-list.css'
})
export class StudentList implements OnInit {


  students$!: Observable<UserDto[]>;


  constructor(
    private userService: UserService
  ) {}


  ngOnInit(): void {

      this.students$ =
          this.userService.getStudents();

  }


  delete(id:number): void {

    if(!confirm('Are you sure you want to delete this student?'))
      return;


    this.userService
      .delete(id)
      .subscribe({

        next: () => {

          this.students$ =
                  this.userService.getStudents();


        },

        error: error => {

          console.error(
            'Failed deleting student',
            error
          );

        }

      });

  }

}