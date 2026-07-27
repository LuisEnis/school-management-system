import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { AssignmentService } 
from '../../../core/services/assignment.service';

import { TeachingAssignmentDto }
from '../../../core/models/assignments/teaching-assignment.dto';
import { Observable } from 'rxjs';



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


assignments$!: Observable<TeachingAssignmentDto[]>;



constructor(
 private assignmentService: AssignmentService
){}



ngOnInit():void {

 this.assignments$ = this.assignmentService.getTeachingAssignments();

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

   this.assignments$ = this.assignmentService.getTeachingAssignments();

  }

 });


}

}