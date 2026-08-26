import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

import { SchoolClass } from '../models/schoolClasses/school-class.model';
import { CreateSchoolClassDto } from '../models/schoolClasses/create-school-class.dto';
import { UpdateSchoolClassDto } from '../models/schoolClasses/update-school-class.dto';
import { ClassDetailsDto } from '../models/schoolClasses/class-details.dto';
import { PagedResult } from '../models/common/paged-result.model';


@Injectable({
  providedIn:'root'
})
export class SchoolClassService {


private apiUrl =
`${environment.apiUrl}/schoolclasses`;


constructor(
 private http:HttpClient
){}



getClasses(
  pageNumber: number = 1,
  pageSize: number = 15,
  search: string = '',
  sortBy: string = '',
  sortDescending: boolean = false
): Observable<PagedResult<SchoolClass>> {

  return this.http.get<PagedResult<SchoolClass>>(
    `${this.apiUrl}?pageNumber=${pageNumber}` +
    `&pageSize=${pageSize}` +
    `&search=${encodeURIComponent(search)}` +
    `&sortBy=${encodeURIComponent(sortBy)}` +
    `&sortDescending=${sortDescending}`
  );

}

getAllClasses():Observable<SchoolClass[]>{

 return this.http.get<SchoolClass[]>(
  `${this.apiUrl}/all`
 );

}



getById(
 id:number
):Observable<SchoolClass>{

 return this.http.get<SchoolClass>(
 `${this.apiUrl}/${id}`
 );

}



getDetails(
 id:number
):Observable<ClassDetailsDto>{

 return this.http.get<ClassDetailsDto>(
 `${this.apiUrl}/${id}/details`
 );

}



create(
 dto:CreateSchoolClassDto
):Observable<SchoolClass>{

 return this.http.post<SchoolClass>(
 this.apiUrl,
 dto
 );

}



update(
 id:number,
 dto:UpdateSchoolClassDto
):Observable<void>{

 return this.http.put<void>(
 `${this.apiUrl}/${id}`,
 dto
 );

}



delete(
 id:number
):Observable<void>{

 return this.http.delete<void>(
 `${this.apiUrl}/${id}`
 );

}

}