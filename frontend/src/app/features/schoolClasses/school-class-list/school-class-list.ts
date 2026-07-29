import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { SchoolClass } from '../../../core/models/schoolClasses/school-class.model';
import { SchoolClassService } from '../../../core/services/schoolClass.service';
import { Observable, startWith, Subject, switchMap } from 'rxjs';


@Component({
  selector:'app-schoolClass-list',
  standalone:true,
  imports:[
    CommonModule,
    RouterLink
  ],
  templateUrl:'./school-cLass-list.html',
  styleUrl:'./school-class-list.css'
})
export class SchoolClassList implements OnInit {

  private reload$ = new Subject<void>();
  schoolClasses$!: Observable<SchoolClass[]>;



  constructor(
    private schoolClassService:SchoolClassService
  ){}



  ngOnInit():void{

    this.schoolClasses$ =
        this.reload$
        .pipe(
            startWith(null),
            switchMap(() =>
                this.schoolClassService.getClasses()
            )
        );

  }


  delete(id:number):void{


    if(!confirm(
      'Delete this class?'
    ))
      return;



    this.schoolClassService
      .delete(id)
      .subscribe({

        next:()=>{

          this.reload$.next();

        },

        error:error=>{

          console.error(
            'Failed deleting class',
            error
          );

        }

      });

  }


}