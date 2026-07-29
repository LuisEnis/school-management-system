import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { AssignmentService } from '../../../core/services/assignment.service';
import { TeacherSubjectAssignmentDto }
from '../../../core/models/assignments/teacher-subject-assignment.dto';
import { Observable, startWith, Subject, switchMap } from 'rxjs';


@Component({
 selector:'app-teacher-subject-assignment',
 standalone:true,
 imports:[
  CommonModule,
  RouterLink
 ],
 templateUrl:'./teacher-subject-assignment.html',
 styleUrl:'./teacher-subject-assignment.css'
})
export class TeacherSubjectAssignment implements OnInit {

 private reload$ = new Subject<void>();
 assignments$!: Observable<TeacherSubjectAssignmentDto[]>;



 constructor(
  private assignmentService: AssignmentService
 ){}



 ngOnInit():void {

  this.assignments$ = 
                   this.reload$
                   .pipe(
                     startWith(null),
                     switchMap(() =>
                       this.assignmentService.getTeacherSubjectAssignments()
                     )
                   );

 }



 delete(
  teacherId:number,
  subjectId:number
 ):void {


  if(!confirm(
   'Remove teacher from subject?'
  ))
   return;



  this.assignmentService
    .removeTeacherFromSubject(
      teacherId,
      subjectId
    )
    .subscribe({

      next:()=>{

        this.reload$.next();

      }

    });

 }

}

function startWIth(arg0: null): import("rxjs").OperatorFunction<void, unknown> {
  throw new Error('Function not implemented.');
}
