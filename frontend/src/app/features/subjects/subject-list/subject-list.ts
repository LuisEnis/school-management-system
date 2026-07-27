import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { SubjectService } from '../../../core/services/subject.service';
import { Subject } from '../../../core/models/subjects/subject.model';
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';


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


  subjects$!: Observable<Subject[]>;


  constructor(
    private subjectService: SubjectService
  ){}



  ngOnInit(): void {

    this.subjects$ = this.subjectService.getAll();

  }


  delete(id:number): void {


    if(!confirm('Are you sure you want to delete this subject?'))
      return;


    this.subjectService
      .delete(id)
      .subscribe({

        next:()=>{

          this.subjects$ = this.subjectService.getAll();

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