import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { AssignmentService } from '../../../core/services/assignment.service';
import { TeacherSubjectAssignmentDto }
from '../../../core/models/assignments/teacher-subject-assignment.dto';
import { Observable, startWith, Subject, Subscription, switchMap } from 'rxjs';
import { SignalRService } from '../../../core/services/signalr.service';


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
export class TeacherSubjectAssignment implements OnInit, OnDestroy {

 private reload$ = new Subject<void>();
 private subscriptions = new Subscription();
 assignments$!: Observable<TeacherSubjectAssignmentDto[]>;



 constructor(
  private assignmentService: AssignmentService,
  private signalRService: SignalRService
 ){}



 ngOnInit():void {

  this.signalRService.startConnection();

  this.subscriptions.add(
    this.signalRService.teacherSubjectAssigned$
      .subscribe(() => {
        this.reload$.next();
      })
  );

  this.subscriptions.add(
    this.signalRService.teacherSubjectRemoved$
      .subscribe(() => {
        this.reload$.next();
      })
  );

  this.assignments$ = 
                   this.reload$
                   .pipe(
                     startWith(null),
                     switchMap(() =>
                       this.assignmentService.getTeacherSubjectAssignments()
                     )
                   );

 }

 ngOnDestroy(): void {
  this.subscriptions.unsubscribe();
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

