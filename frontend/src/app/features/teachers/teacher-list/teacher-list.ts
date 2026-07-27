import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { UserService } from '../../../core/services/user.service';
import { UserDto } from '../../../core/models/users/user.dto';
import { Observable } from 'rxjs';


@Component({
  selector:'app-teacher-list',
  standalone:true,
  imports:[
    CommonModule,
    RouterLink
  ],
  templateUrl:'./teacher-list.html',
  styleUrl:'./teacher-list.css'
})
export class TeacherList implements OnInit {


teachers$!: Observable<UserDto[]>;


constructor(
 private userService:UserService
){}



ngOnInit():void{

 this.teachers$ = this.userService.getTeachers();

}



delete(id:number):void{


 if(!confirm(
  'Are you sure you want to delete this teacher?'
 ))
 return;


 this.userService
 .delete(id)
 .subscribe({

    next:()=>{

        this.teachers$ = this.userService.getTeachers();

    },

    error:error=>{

        console.error(error);

    }

 });


}

}