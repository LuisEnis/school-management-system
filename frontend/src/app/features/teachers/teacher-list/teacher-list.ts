import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { UserService } from '../../../core/services/user.service';
import { UserDto } from '../../../core/models/users/user.dto';
import { debounceTime, distinctUntilChanged, map, Observable, startWith, Subject, Subscription, switchMap, tap } from 'rxjs';
import { AuthService } from '../../../core/services/auth.service';
import { PaginationComponent } from '../../../shared/components/pagination/pagination';
import { PagedResult } from '../../../core/models/common/paged-result.model';
import { SignalRService } from '../../../core/services/signalr.service';


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
export class TeacherList implements OnInit, OnDestroy  {

private subscriptions = new Subscription();
private reload$ = new Subject<void>();
private search$ = new Subject<string>();
teachers$!: Observable<UserDto[]>;
pageNumber = 1;
pageSize = 15;
totalPages = 0;
totalCount = 0;
searchTerm = '';
sortBy = '';
sortDescending = false;


constructor(
 private userService:UserService,
 public authService: AuthService,
 private signalRService: SignalRService
){}



ngOnInit():void{

  this.signalRService.startConnection();

  this.subscriptions.add(
    this.signalRService.teacherCreated$
      .subscribe(() => {
        this.reload$.next();
      })
  );

  this.subscriptions.add(
    this.signalRService.teacherUpdated$
      .subscribe(() => {
        this.reload$.next();
      })
  );

  this.subscriptions.add(
    this.signalRService.teacherDeleted$
      .subscribe(() => {
        this.reload$.next();
      })
  );

  this.subscriptions.add(
    this.search$
      .pipe(
        debounceTime(300),
        distinctUntilChanged()
      )
      .subscribe(search => {
        this.searchTerm = search;
        this.pageNumber = 1;
        this.reload$.next();
      })
  );

  this.teachers$ =
                this.reload$
                .pipe(
                    startWith(null),
                    switchMap(() =>
                        this.userService.getTeachers(this.pageNumber, this.pageSize, this.searchTerm, this.sortBy, this.sortDescending)
                    ),
                    tap((result: PagedResult<UserDto>) => {
  
                      this.totalPages = result.totalPages;
                      this.totalCount = result.totalCount;
  
                    }),
  
                    map(result => result.items)
                );
}

ngOnDestroy(): void {
  this.subscriptions.unsubscribe();
}

onSearch(search: string): void {

  this.search$.next(search);

}


onSort(column: string): void {

  if (this.sortBy === column) {

    this.sortDescending = !this.sortDescending;

  } else {

    this.sortBy = column;
    this.sortDescending = false;

  }

  this.pageNumber = 1;

  this.reload$.next();

}

getSortIcon(column: string): string {

  if (this.sortBy !== column)
    return '↕';

  return this.sortDescending
    ? '↓'
    : '↑';

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

