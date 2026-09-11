import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { UserService } from '../../../core/services/user.service';
import { SignalRService } from '../../../core/services/signalr.service';
import { UserDto } from '../../../core/models/users/user.dto';
import { debounceTime, distinctUntilChanged, map, Observable, startWith, Subject, Subscription, switchMap, tap } from 'rxjs';
import { PaginationComponent } from '../../../shared/components/pagination/pagination';
import { PagedResult } from '../../../core/models/common/paged-result.model';


@Component({
  selector: 'app-student-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    PaginationComponent
  ],
  templateUrl: './student-list.html',
  styleUrl: './student-list.css'
})
export class StudentList implements OnInit, OnDestroy  {

  private reload$ = new Subject<void>();
  private search$ = new Subject<string>();
  private subscriptions = new Subscription();
  students$!: Observable<UserDto[]>;
  pageNumber = 1;
  pageSize = 15;
  totalPages = 0;
  totalCount = 0;
  search = '';
  sortBy = '';
  sortDescending = false;


  constructor(
    private userService: UserService,
    private signalRService: SignalRService
  ) {}


  ngOnInit(): void {

    this.signalRService.startConnection();

    this.subscriptions.add(
      this.signalRService.studentCreated$
        .subscribe(() => {
          this.reload$.next();
        })
    );

    this.subscriptions.add(
      this.signalRService.studentUpdated$
        .subscribe(() => {
          this.reload$.next();
        })
    );

    this.subscriptions.add(
      this.signalRService.studentDeleted$
        .subscribe(() => {
          this.reload$.next();
        })
    );

    this.subscriptions.add(
      this.search$
        .pipe(
          debounceTime(400),
          distinctUntilChanged()
        )
        .subscribe(search => {
          this.search = search;
          this.pageNumber = 1;

          this.reload$.next();
        })
    );

      this.students$ =
              this.reload$
              .pipe(
                  startWith(null),
                  switchMap(() =>
                      this.userService.getStudents(this.pageNumber, this.pageSize, this.search, this.sortBy, this.sortDescending)
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

  onSearch(event: Event): void {

    const input =
      event.target as HTMLInputElement;

    this.search$.next(input.value);

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


  delete(id:number): void {

    if(!confirm('Are you sure you want to delete this student?'))
      return;


    this.userService
      .delete(id)
      .subscribe({

        next: () => {

          this.reload$.next();


        },

        error: error => {

          console.error(
            'Failed deleting student',
            error
          );

        }

      });

  }

}

