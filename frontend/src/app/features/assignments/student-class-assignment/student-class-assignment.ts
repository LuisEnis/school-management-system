import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { AssignmentService } from '../../../core/services/assignment.service';
import { StudentClassAssignmentDto } 
from '../../../core/models/assignments/student-class-assignment.dto';
import { Observable, startWith, Subject, switchMap } from 'rxjs';


@Component({
  selector:'app-student-class-assignment',
  standalone:true,
  imports:[
    CommonModule,
    RouterLink
  ],
  templateUrl:'./student-class-assignment.html',
  styleUrl:'./student-class-assignment.css'
})
export class StudentClassAssignment implements OnInit {

  private reload$ = new Subject<void>();
  assignments$!: Observable<StudentClassAssignmentDto[]>;


  constructor(
    private assignmentService: AssignmentService
  ){}



  ngOnInit():void {

    this.assignments$ = 
                 this.reload$
                 .pipe(
                   startWith(null),
                   switchMap(() =>
                     this.assignmentService.getStudentClassAssignments()
                   )
                 );

  }



  delete(
    studentId:number,
    schoolClassId:number
  ):void {


    if(!confirm(
      'Remove student from class?'
    ))
      return;



    this.assignmentService
      .removeStudentFromClass(
        studentId,
        schoolClassId
      )
      .subscribe({

        next:()=>{

          this.reload$.next();

        },

        error:error=>{

          console.error(
            'Failed deleting assignment',
            error
          );

        }

      });

  }

}

function startWIth(arg0: null): import("rxjs").OperatorFunction<void, unknown> {
  throw new Error('Function not implemented.');
}
