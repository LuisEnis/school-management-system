import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { SchoolClass } from '../../../core/models/school-class.model';
import { SchoolClassService } from '../../../core/services/schoolClass.service';

@Component({
  selector: 'app-schoolClass-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './school-cLass-list.html',
  styleUrl: './school-class-list.css'
})
export class SchoolClassList implements OnInit {

  schoolClasses: SchoolClass[] = [];

  constructor(private schoolClassService: SchoolClassService) {}

  ngOnInit(): void {
    this.schoolClasses = this.schoolClassService.getClasses();
  }

  delete(id: number) {
    this.schoolClassService.delete(id);
    this.schoolClasses = this.schoolClassService.getClasses();
  }
}