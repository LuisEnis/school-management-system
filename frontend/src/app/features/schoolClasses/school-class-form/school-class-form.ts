import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SchoolClassService } from '../../../core/services/schoolClass.service';

@Component({
  selector: 'app-schoolClass-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './school-class-form.html',
  styleUrl: './school-class-form.css'
})
export class SchoolClassForm implements OnInit {

  form!: FormGroup;
  schoolClassId: number | null = null;
  isEditMode = false;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private schoolClassService: SchoolClassService
  ) {}

  ngOnInit(): void {

    this.form = this.fb.group({
      name: ['', Validators.required]
    });

    this.schoolClassId = Number(this.route.snapshot.paramMap.get('id'));

    if (this.schoolClassId) {
      this.isEditMode = true;

      const schoolClass = this.schoolClassService.getById(this.schoolClassId);

      if (schoolClass) {
        this.form.patchValue(schoolClass);
      }
    }
  }

  save(): void {

    if (this.isEditMode) {
      this.schoolClassService.update(this.schoolClassId!, this.form.value);
    } else {
      this.schoolClassService.add({
        id: 0,
        ...this.form.value
      });
    }

    this.router.navigate(['/schoolClasses']);
  }
}