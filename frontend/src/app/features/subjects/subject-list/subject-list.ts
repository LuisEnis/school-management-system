import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { SubjectService } from '../../../core/services/subject.service';
import { Subject } from '../../../core/models/subject.model';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-subject-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './subject-list.html',
  styleUrl: './subject-list.css'
})
export class SubjectList implements OnInit {

  subjects: Subject[] = [];

  constructor(private subjectService: SubjectService) {}

  ngOnInit(): void {
    this.subjects = this.subjectService.getSubjects();
  }

  delete(id: number) {
    this.subjectService.deleteSubject(id);
    this.subjects = this.subjectService.getSubjects();
  }
}