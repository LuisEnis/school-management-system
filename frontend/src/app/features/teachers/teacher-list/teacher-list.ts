import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { UserService } from '../../../core/services/user.service';
import { UserDto } from '../../../core/models/users/user.dto';
import { map, Observable, startWith, Subject, switchMap, tap } from 'rxjs';
import { AuthService } from '../../../core/services/auth.service';
import { PaginationComponent } from '../../../shared/components/pagination/pagination';
import { PagedResult } from '../../../core/models/common/paged-result.model';


@Component({
  selector:'app-teacher-list',
  standalone:true,
  imports:[
    CommonModule,
    RouterLink,
    PaginationComponent
  ],
  templateUrl:'./teacher-list.html',
  styleUrl:'./teacher-list.css'
})
export class TeacherList implements OnInit {

private reload$ = new Subject<void>();
teachers$!: Observable<UserDto[]>;
pageNumber = 1;
pageSize = 15;
totalPages = 0;
totalCount = 0;


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
                        this.userService.getTeachers(this.pageNumber, this.pageSize)
                    ),
                    tap((result: PagedResult<UserDto>) => {
  
                      this.totalPages = result.totalPages;
                      this.totalCount = result.totalCount;
  
                    }),
  
                    map(result => result.items)
                );
}

onPageChange(page: number): void {

  this.pageNumber = page;

  this.reload$.next();

}


onPageSizeChange(size: number): void {

  this.pageSize = size;
  this.pageNumber = 1;

  this.reload$.next();

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

