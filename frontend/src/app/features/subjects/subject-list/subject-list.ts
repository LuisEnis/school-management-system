import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { SubjectService } from '../../../core/services/subject.service';
import { Subject as SubjectModel } from '../../../core/models/subjects/subject.model';
import { Component, OnInit } from '@angular/core';
import { Observable, startWith, Subject, switchMap } from 'rxjs';


@Component({
  selector: 'app-subject-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink
  ],
  templateUrl: './subject-list.html',
  styleUrl: './subject-list.css'
})
export class SubjectList implements OnInit {

  private reload$ = new Subject<void>();
  subjects$!: Observable<SubjectModel[]>;


  constructor(
    private subjectService: SubjectService
  ){}



  ngOnInit(): void {

    this.subjects$ = 
      this.reload$
      .pipe(
        startWith(null),
          switchMap(() =>
          this.subjectService.getAll()
        )
      );

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
