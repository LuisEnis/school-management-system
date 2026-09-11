import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { AssignmentService } from '../../../core/services/assignment.service';
import { StudentClassAssignmentDto } 
from '../../../core/models/assignments/student-class-assignment.dto';
import { Observable, startWith, Subject, Subscription, switchMap } from 'rxjs';
import { SignalRService } from '../../../core/services/signalr.service';


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
export class StudentClassAssignment implements OnInit, OnDestroy {

  private reload$ = new Subject<void>();
  private subscriptions = new Subscription();
  assignments$!: Observable<StudentClassAssignmentDto[]>;


  constructor(
    private assignmentService: AssignmentService,
    private signalRService: SignalRService
  ){}



  ngOnInit():void {

    this.signalRService.startConnection();

    this.subscriptions.add(
      this.signalRService.studentClassAssigned$
        .subscribe(() => {
          this.reload$.next();
        })
    );

    this.subscriptions.add(
      this.signalRService.studentClassRemoved$
        .subscribe(() => {
          this.reload$.next();
        })
    );

    this.assignments$ = 
                 this.reload$
                 .pipe(
                   startWith(null),
                   switchMap(() =>
                     this.assignmentService.getStudentClassAssignments()
                   )
                 );

  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
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

