import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { UserService } from '../../../core/services/user.service';
import { UserDto } from '../../../core/models/users/user.dto';
import { Observable, startWith, Subject, switchMap } from 'rxjs';
import { AuthService } from '../../../core/services/auth.service';


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

private reload$ = new Subject<void>();
teachers$!: Observable<UserDto[]>;


constructor(
 private userService:UserService,
 public authService: AuthService
){}



ngOnInit():void{

  this.teachers$ =
                this.reload$
                .pipe(
                    startWith(null),
                    switchMap(() =>
                        this.userService.getTeachers()
                    )
                );

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

        this.reload$.next();

    },

    error:error=>{

        console.error(error);

    }

 });


}

}

function startWIth(arg0: null): import("rxjs").OperatorFunction<void, unknown> {
  throw new Error('Function not implemented.');
}
