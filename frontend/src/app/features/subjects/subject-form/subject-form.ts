import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SubjectService } from '../../../core/services/subject.service';
import { Subject } from '../../../core/models/subject.model';

@Component({
  selector: 'app-subject-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
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
  ) {}

  ngOnInit(): void {

    this.form = this.fb.group({
      name: ['', Validators.required]
    });

    this.subjectId = Number(this.route.snapshot.paramMap.get('id'));

    if (this.subjectId) {
      this.isEditMode = true;

      const subject = this.subjectService.getSubjectById(this.subjectId);

      if (subject) {
        this.form.patchValue(subject);
      }
    }
  }

  save(): void {

    if (this.isEditMode) {
      this.subjectService.updateSubject(this.subjectId!, this.form.value);
    } else {
      this.subjectService.addSubject({
        id: 0,
        ...this.form.value
      });
    }

    this.router.navigate(['/subjects']);
  }
}