import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ReactiveFormsModule,
  FormGroup,
  FormBuilder,
  Validators
} from '@angular/forms';
import {
  ActivatedRoute,
  Router
} from '@angular/router';

import { SubjectService } from '../../../core/services/subject.service';
import { Subject } from '../../../core/models/subjects/subject.model';


@Component({
  selector: 'app-subject-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './subject-form.html',
  styleUrl: './subject-form.css'
})
export class SubjectForm implements OnInit {


  form!: FormGroup;

  subjectId: number | null = null;

  isEditMode = false;


  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private subjectService: SubjectService
  ){}



  ngOnInit(): void {


    this.form = this.fb.group({

      name:[
        '',
        Validators.required
      ]

    });



    const id =
      this.route.snapshot.paramMap.get('id');


    if(id){

      this.subjectId = Number(id);

      this.isEditMode = true;


      this.subjectService
        .getById(this.subjectId)
        .subscribe({

          next: subject => {

            this.form.patchValue(subject);

          }

        });

    }

  }




  save(): void {


    if(this.form.invalid)
      return;



    if(this.isEditMode){

      this.subjectService
        .update(
          this.subjectId!,
          this.form.value
        )
        .subscribe({

          next: () => {

            this.router.navigate(['/subjects']);

          }

        });


    }
    else{


      this.subjectService
        .create(this.form.value)
        .subscribe({

          next: () => {

            this.router.navigate(['/subjects']);

          }

        });

    }


  }


}