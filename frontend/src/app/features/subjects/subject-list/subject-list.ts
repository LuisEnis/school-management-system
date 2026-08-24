import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { SubjectService } from '../../../core/services/subject.service';
import { Subject as SubjectModel } from '../../../core/models/subjects/subject.model';
import { Component, OnInit } from '@angular/core';
import { map, Observable, startWith, Subject, switchMap, tap } from 'rxjs';
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
  subjects$!: Observable<SubjectModel[]>;
  pageNumber = 1;
  pageSize = 15;
  totalPages = 0;
  totalCount = 0;


  constructor(
    private subjectService: SubjectService
  ){}



  ngOnInit(): void {

    this.subjects$ = 
      this.reload$
      .pipe(
        startWith(null),
          switchMap(() =>
          this.subjectService.getAll(this.pageNumber, this.pageSize)
        ),
        tap((result: PagedResult<SubjectModel>) => {
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

function startWIth(arg0: null): import("rxjs").OperatorFunction<void, unknown> {
  throw new Error('Function not implemented.');
}
