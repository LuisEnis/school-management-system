import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { SchoolClass } from '../../../core/models/schoolClasses/school-class.model';
import { SchoolClassService } from '../../../core/services/schoolClass.service';
import { debounceTime, distinctUntilChanged, map, Observable, shareReplay, startWith, Subject, switchMap, tap } from 'rxjs';
import { PagedResult } from '../../../core/models/common/paged-result.model';
import { PaginationComponent } from '../../../shared/components/pagination/pagination';


@Component({
  selector:'app-school-class-list',
  standalone:true,
  imports:[
    CommonModule,
    RouterLink,
    PaginationComponent
  ],
  templateUrl:'./school-class-list.html',
  styleUrl:'./school-class-list.css'
})
export class SchoolClassList implements OnInit {

  private reload$ = new Subject<void>();
  private search$ = new Subject<string>();
  schoolClasses$!: Observable<SchoolClass[]>;
  pageNumber = 1;
  pageSize = 15;
  totalPages = 0;
  totalCount = 0;
  searchTerm = '';
  sortBy = 'name';
  sortDescending = false;



  constructor(
    private schoolClassService:SchoolClassService
  ){}



  ngOnInit():void{

    this.search$
      .pipe(
        debounceTime(300),
        distinctUntilChanged()
      )
      .subscribe(search => {

        this.searchTerm = search;
        this.pageNumber = 1;

        this.reload$.next();

      });

    this.schoolClasses$ =
        this.reload$
        .pipe(
            startWith(null),
            switchMap(() =>
              this.schoolClassService.getClasses(this.pageNumber, this.pageSize, this.searchTerm, this.sortBy, this.sortDescending)
            ),
            tap((result: PagedResult<SchoolClass>) => {

            this.totalPages = result.totalPages;
            this.totalCount = result.totalCount;

          }),
          map(result =>
            result.items
          ),

          shareReplay(1)
        );

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


  delete(id: number): void {

    if (!confirm('Delete this class?'))
      return;


    this.schoolClassService
      .delete(id)
      .subscribe({

        next: () => {

          this.reload$.next();

        },

        error: error => {

          console.error(
            'Failed deleting class',
            error
          );

        }

      });

  }


}