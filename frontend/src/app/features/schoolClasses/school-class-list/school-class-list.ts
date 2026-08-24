import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { SchoolClass } from '../../../core/models/schoolClasses/school-class.model';
import { SchoolClassService } from '../../../core/services/schoolClass.service';
import { map, Observable, shareReplay, startWith, Subject, switchMap, tap } from 'rxjs';
import { PagedResult } from '../../../core/models/common/paged-result.model';
import { PaginationComponent } from '../../../shared/components/pagination/pagination';


@Component({
  selector:'app-schoolClass-list',
  standalone:true,
  imports:[
    CommonModule,
    RouterLink,
    PaginationComponent
  ],
  templateUrl:'./school-cLass-list.html',
  styleUrl:'./school-class-list.css'
})
export class SchoolClassList implements OnInit {

  private reload$ = new Subject<void>();
  schoolClasses$!: Observable<SchoolClass[]>;
  pageNumber = 1;
  pageSize = 15;
  totalPages = 0;
  totalCount = 0;



  constructor(
    private schoolClassService:SchoolClassService
  ){}



  ngOnInit():void{

    this.schoolClasses$ =
        this.reload$
        .pipe(
            startWith(null),
            switchMap(() =>
              this.schoolClassService.getClasses(this.pageNumber, this.pageSize)
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