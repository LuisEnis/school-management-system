import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
 FormBuilder,
 FormGroup,
 ReactiveFormsModule,
 Validators
} from '@angular/forms';

import { Router } from '@angular/router';

import { UserService } from '../../../core/services/user.service';
import { SubjectService } from '../../../core/services/subject.service';
import { AssignmentService } from '../../../core/services/assignment.service';

import { UserDto } from '../../../core/models/users/user.dto';
import { Subject } from '../../../core/models/subjects/subject.model';


@Component({
selector:'app-teacher-subject-assignment-form',
standalone:true,
imports:[
 CommonModule,
 ReactiveFormsModule
],
templateUrl:'./teacher-subject-assignment-form.html',
styleUrl:'./teacher-subject-assignment-form.css'
})
export class TeacherSubjectAssignmentForm implements OnInit{


form!:FormGroup;

teachers:UserDto[]=[];

subjects:Subject[]=[];



constructor(
private fb:FormBuilder,
private userService:UserService,
private subjectService:SubjectService,
private assignmentService:AssignmentService,
private router:Router
){}



ngOnInit():void{


this.form=this.fb.group({

 teacherId:[
  null,
  Validators.required
 ],

 subjectId:[
  null,
  Validators.required
 ]

});


this.userService
.getTeachers()
.subscribe(x=>this.teachers=x);


this.subjectService
.getAll()
.subscribe(x=>this.subjects=x);


}



save():void{


this.assignmentService
.assignTeacherToSubject(
 this.form.value
)
.subscribe(()=>{

 this.router.navigate(
  ['/assignments/teacher-subject']
 );

});


}


}