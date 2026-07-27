import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { Subject } from '../models/subjects/subject.model';
import { CreateSubjectDto } from '../models/subjects/create-subject.dto';
import { UpdateSubjectDto } from '../models/subjects/update-subject.dto';


@Injectable({
  providedIn: 'root'
})
export class SubjectService {


  private apiUrl = `${environment.apiUrl}/subjects`;


  constructor(
    private http: HttpClient
  ) {}


  getAll(): Observable<Subject[]> {

    return this.http.get<Subject[]>(
      this.apiUrl
    );

  }


  getById(id:number): Observable<Subject> {

    return this.http.get<Subject>(
      `${this.apiUrl}/${id}`
    );

  }


  create(
    dto: CreateSubjectDto
  ): Observable<Subject> {

    return this.http.post<Subject>(
      this.apiUrl,
      dto
    );

  }


  update(
    id:number,
    dto: UpdateSubjectDto
  ): Observable<void> {

    return this.http.put<void>(
      `${this.apiUrl}/${id}`,
      dto
    );

  }


  delete(
    id:number
  ): Observable<void> {

    return this.http.delete<void>(
      `${this.apiUrl}/${id}`
    );

  }

}