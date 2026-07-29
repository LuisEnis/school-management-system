import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

import { SchoolClassService } 
from '../../../core/services/schoolClass.service';

import { ClassDetailsDto }
from '../../../core/models/schoolClasses/class-details.dto';
import { Observable } from 'rxjs';
import { AuthService } from '../../../core/services/auth.service';



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


details$!: Observable<ClassDetailsDto>;


constructor(
 private route:ActivatedRoute,
 private schoolClassService:SchoolClassService,
 public authService: AuthService
){}




ngOnInit():void{


 const id =
 Number(
  this.route.snapshot.paramMap.get('id')
 );


 this.details$ =
    this.schoolClassService.getDetails(id);


}



}