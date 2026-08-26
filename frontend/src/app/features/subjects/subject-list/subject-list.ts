import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { SubjectService } from '../../../core/services/subject.service';
import { Subject as SubjectModel } from '../../../core/models/subjects/subject.model';
import { Component, OnInit } from '@angular/core';
import { debounceTime, distinctUntilChanged, map, Observable, startWith, Subject, switchMap, tap } from 'rxjs';
import { PaginationComponent } from '../../../shared/components/pagination/pagination';
import { PagedResult } from '../../../core/models/common/paged-result.model';


@Component({
  selector: 'app-subject-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    PaginationComponent
  ],
  templateUrl: './subject-list.html',
  styleUrl: './subject-list.css'
})
export class SubjectList implements OnInit {

  private reload$ = new Subject<void>();
  private search$ = new Subject<string>();
  subjects$!: Observable<SubjectModel[]>;
  pageNumber = 1;
  pageSize = 15;
  totalPages = 0;
  totalCount = 0;
  searchTerm = '';
  sortBy = 'name';
  sortDescending = false;


  constructor(
    private subjectService: SubjectService
  ){}



  ngOnInit(): void {

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

    this.subjects$ = 
      this.reload$
      .pipe(
        startWith(null),
          switchMap(() =>
          this.subjectService.getAll(this.pageNumber, this.pageSize, this.searchTerm, this.sortBy, this.sortDescending)
        ),
        tap((result: PagedResult<SubjectModel>) => {
          this.totalPages = result.totalPages;
          this.totalCount = result.totalCount;
        }),
        map(result => result.items)
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


  delete(id:number): void {


    if(!confirm('Are you sure you want to delete this subject?'))
      return;


    this.subjectService
      .delete(id)
      .subscribe({

        next:()=>{

          this.reload$.next();

        },

        error:error=>{

          console.error(
            'Failed deleting subject',
            error
          );

        }

      });

  }

}

