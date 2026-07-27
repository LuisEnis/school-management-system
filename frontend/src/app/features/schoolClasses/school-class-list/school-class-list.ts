import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { SchoolClass } from '../../../core/models/schoolClasses/school-class.model';
import { SchoolClassService } from '../../../core/services/schoolClass.service';
import { Observable } from 'rxjs';


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


  schoolClasses$!: Observable<SchoolClass[]>;



  constructor(
    private schoolClassService:SchoolClassService
  ){}



  ngOnInit():void{

    this.schoolClasses$ = this.schoolClassService.getClasses();

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

          this.schoolClasses$ = this.schoolClassService.getClasses();

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