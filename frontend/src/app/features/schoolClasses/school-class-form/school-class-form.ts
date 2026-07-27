import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators
} from '@angular/forms';

import {
  ActivatedRoute,
  Router
} from '@angular/router';

import { SchoolClassService } from '../../../core/services/schoolClass.service';



@Component({
  selector:'app-schoolClass-form',
  standalone:true,
  imports:[
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl:'./school-class-form.html',
  styleUrl:'./school-class-form.css'
})
export class SchoolClassForm implements OnInit {


  form!:FormGroup;


  schoolClassId:number|null=null;


  isEditMode=false;




  constructor(
    private fb:FormBuilder,
    private route:ActivatedRoute,
    private router:Router,
    private schoolClassService:SchoolClassService
  ){}




  ngOnInit():void{


    this.form=this.fb.group({

      name:[
        '',
        Validators.required
      ]

    });



    const id =
      this.route.snapshot.paramMap.get('id');



    if(id){

      this.schoolClassId=Number(id);

      this.isEditMode=true;



      this.schoolClassService
        .getById(this.schoolClassId)
        .subscribe({

          next:schoolClass=>{

            this.form.patchValue(
              schoolClass
            );

          }

        });


    }


  }





  save():void{


    if(this.form.invalid)
      return;



    if(this.isEditMode){


      this.schoolClassService
        .update(
          this.schoolClassId!,
          this.form.value
        )
        .subscribe({

          next:()=>{

            this.router.navigate(
              ['/schoolClasses']
            );

          }

        });



    }
    else{


      this.schoolClassService
        .create(
          this.form.value
        )
        .subscribe({

          next:()=>{

            this.router.navigate(
              ['/schoolClasses']
            );

          }

        });


    }



  }


}