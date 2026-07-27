import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

import { SchoolClassService } 
from '../../../core/services/schoolClass.service';

import { ClassDetailsDto }
from '../../../core/models/schoolClasses/class-details.dto';



@Component({
  selector:'app-class-details',
  standalone:true,
  imports:[
    CommonModule
  ],
  templateUrl:'./class-details.html',
  styleUrl:'./class-details.css'
})
export class ClassDetails implements OnInit {


details!:ClassDetailsDto;


constructor(
 private route:ActivatedRoute,
 private schoolClassService:SchoolClassService
){}




ngOnInit():void{


 const id =
 Number(
  this.route.snapshot.paramMap.get('id')
 );


 this.schoolClassService
 .getDetails(id)
 .subscribe({

  next:data=>{

   this.details=data;

  },

  error:error=>{

   console.error(
    'Failed loading class details',
    error
   );

  }

 });


}



}