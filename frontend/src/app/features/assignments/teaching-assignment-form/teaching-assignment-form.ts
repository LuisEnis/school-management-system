import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
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
import { SchoolClassService } from '../../../core/services/schoolClass.service';
import { AssignmentService } from '../../../core/services/assignment.service';


import { UserDto } from '../../../core/models/users/user.dto';
import { Subject } from '../../../core/models/subjects/subject.model';
import { SchoolClass } from '../../../core/models/schoolClasses/school-class.model';
import { forkJoin } from 'rxjs';


@Component({
selector:'app-teaching-assignment-form',
standalone:true,
imports:[
 CommonModule,
 ReactiveFormsModule
],
templateUrl:'./teaching-assignment-form.html',
styleUrl:'./teaching-assignment-form.css'
})
export class TeachingAssignmentForm implements OnInit{


form!:FormGroup;


teachers:UserDto[]=[];

subjects:Subject[]=[];

classes:SchoolClass[]=[];



constructor(
private fb:FormBuilder,
private userService:UserService,
private subjectService:SubjectService,
private schoolClassService:SchoolClassService,
private assignmentService:AssignmentService,
private router:Router,
private cdr:ChangeDetectorRef
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
 ],

 schoolClassId:[
  null,
  Validators.required
 ]

});

this.loadData();


}


loadData(): void {

  forkJoin({

    teachers: this.userService.getAllTeachers(),

    subjects: this.subjectService.getAllSubjects(),

    classes: this.schoolClassService.getAllClasses()

  }).subscribe({

    next: data => {

      this.teachers = data.teachers;
      this.subjects = data.subjects;
      this.classes = data.classes;

      this.cdr.detectChanges();

    },

    error: error => {

      console.error(
        'Failed loading assignment data',
        error
      );

    }

  });

}


save():void{


this.assignmentService
.assignTeachingAssignment(
 this.form.value
)
.subscribe(()=>{


this.router.navigate(
 ['/assignments/teaching-assignment']
);


});


}


}