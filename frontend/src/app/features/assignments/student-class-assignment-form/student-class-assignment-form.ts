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
import { SchoolClassService } from '../../../core/services/schoolClass.service';
import { AssignmentService } from '../../../core/services/assignment.service';

import { UserDto } from '../../../core/models/users/user.dto';
import { SchoolClass } from '../../../core/models/schoolClasses/school-class.model';


@Component({
  selector: 'app-student-class-assignment-form',
  standalone:true,
  imports:[
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl:'./student-class-assignment-form.html',
  styleUrl:'./student-class-assignment-form.css'
})
export class StudentClassAssignmentForm implements OnInit {


form!:FormGroup;

students:UserDto[] = [];

classes:SchoolClass[] = [];


constructor(
 private fb:FormBuilder,
 private userService:UserService,
 private schoolClassService:SchoolClassService,
 private assignmentService:AssignmentService,
 private router:Router
){}



ngOnInit():void{


 this.form=this.fb.group({

  studentId:[
    null,
    Validators.required
  ],

  schoolClassId:[
    null,
    Validators.required
  ]

 });


 this.loadData();

}



loadData():void{


 this.userService
 .getStudents()
 .subscribe(data=>{

  this.students=data;

 });



 this.schoolClassService
 .getClasses()
 .subscribe(data=>{

  this.classes=data;

 });


}



save():void{


 this.assignmentService
 .assignStudentToClass(
   this.form.value
 )
 .subscribe({

  next:()=>{

   this.router.navigate(
    ['/assignments/student-class']
   );

  }

 });


}



}