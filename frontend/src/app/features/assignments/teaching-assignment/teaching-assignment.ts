import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { AssignmentService } 
from '../../../core/services/assignment.service';

import { TeachingAssignmentDto }
from '../../../core/models/assignments/teaching-assignment.dto';
import { Observable, startWith, Subject, switchMap } from 'rxjs';



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
export class TeachingAssignment implements OnInit {

private reload$ = new Subject<void>();
assignments$!: Observable<TeachingAssignmentDto[]>;



constructor(
 private assignmentService: AssignmentService
){}



ngOnInit():void {

 this.assignments$ = 
                    this.reload$
                    .pipe(
                      startWith(null),
                      switchMap(() =>
                        this.assignmentService.getTeachingAssignments()
                      )
                    );

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

function startWIth(arg0: null): import("rxjs").OperatorFunction<void, unknown> {
    throw new Error('Function not implemented.');
}
