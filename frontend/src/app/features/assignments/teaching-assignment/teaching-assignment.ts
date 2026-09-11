import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { AssignmentService } 
from '../../../core/services/assignment.service';

import { TeachingAssignmentDto }
from '../../../core/models/assignments/teaching-assignment.dto';
import { Observable, startWith, Subject, Subscription, switchMap } from 'rxjs';
import { SignalRService } from '../../../core/services/signalr.service';



@Component({
 selector:'app-teaching-assignment',
 standalone:true,
 imports:[
  CommonModule,
  RouterLink
 ],
 templateUrl:'./teaching-assignment.html',
 styleUrl:'./teaching-assignment.css'
})
export class TeachingAssignment implements OnInit, OnDestroy {

private reload$ = new Subject<void>();
private subscriptions = new Subscription();
assignments$!: Observable<TeachingAssignmentDto[]>;



constructor(
 private assignmentService: AssignmentService,
 private signalRService: SignalRService
){}



ngOnInit():void {

  this.signalRService.startConnection();

  this.subscriptions.add(
    this.signalRService.teachingAssignmentCreated$
      .subscribe(() => {
        this.reload$.next();
      })
  );

  this.subscriptions.add(
    this.signalRService.teachingAssignmentRemoved$
      .subscribe(() => {
        this.reload$.next();
      })
  );

  this.assignments$ = 
                    this.reload$
                    .pipe(
                      startWith(null),
                      switchMap(() =>
                        this.assignmentService.getTeachingAssignments()
                      )
                    );

}

 ngOnDestroy(): void {
  this.subscriptions.unsubscribe();
 }

delete(
 schoolClassId:number,
 subjectId:number,
 teacherId:number
):void {


if(!confirm(
 'Remove teaching assignment?'
))
 return;



this.assignmentService
 .removeTeachingAssignment(
  schoolClassId,
  subjectId,
  teacherId
 )
 .subscribe({

  next:()=>{

   this.reload$.next();

  }

 });


}

}

