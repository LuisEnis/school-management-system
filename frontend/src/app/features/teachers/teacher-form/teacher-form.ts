import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { UserService } from '../../../core/services/user.service';
import { UserRole } from '../../../core/models/user.model';
import { UserDetails } from '../../../core/models/users/user-details.dto';
import { UpdateUserDto } from '../../../core/models/users/update-user.dto';
import { CreateUserDto } from '../../../core/models/users/create-user.dto';



@Component({
  selector: 'app-teacher-form',
  standalone:true,
  imports:[
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl:'./teacher-form.html',
  styleUrl:'./teacher-form.css'
})
export class TeacherForm implements OnInit {


form!:FormGroup;

teacherId:number|null=null;

isEditMode=false;



constructor(
 private fb:FormBuilder,
 private route:ActivatedRoute,
 private router:Router,
 private userService:UserService
){}



ngOnInit():void {


this.form=this.fb.group({

 username:[
   '',
   Validators.required
 ],

 firstName:[
   '',
   Validators.required
 ],

 lastName:[
   '',
   Validators.required
 ],

 email:[
   '',
   [
    Validators.required,
    Validators.email
   ]
 ],

 password:[
   ''
 ]

});


this.teacherId =
Number(
 this.route.snapshot.paramMap.get('id')
);



if(this.teacherId){


 this.isEditMode=true;


 this.userService
   .getUserById(this.teacherId)
   .subscribe({

    next: teacher => {

      this.form.patchValue(teacher);
    }

   });

}


}




save():void {


if(this.isEditMode){


 const dto:UpdateUserDto =
 {
   ...this.form.value,
   role:UserRole.Teacher
 };


 this.userService
   .update(
      this.teacherId!,
      dto
   )
   .subscribe({

     next:()=>{

       this.router.navigate(['/teachers']);

     }

   });



}
else{


const dto:CreateUserDto =
{
 ...this.form.value,
 role:UserRole.Teacher
};



this.userService
.create(dto)
.subscribe({

 next:()=>{

   this.router.navigate(['/teachers']);

 }

});


}



}


}