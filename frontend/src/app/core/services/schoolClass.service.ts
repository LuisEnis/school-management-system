import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

import { SchoolClass } from '../models/schoolClasses/school-class.model';
import { CreateSchoolClassDto } from '../models/schoolClasses/create-school-class.dto';
import { UpdateSchoolClassDto } from '../models/schoolClasses/update-school-class.dto';
import { ClassDetailsDto } from '../models/schoolClasses/class-details.dto';


@Injectable({
  providedIn:'root'
})
export class SchoolClassService {


private apiUrl =
`${environment.apiUrl}/schoolclasses`;


constructor(
 private http:HttpClient
){}



getClasses():Observable<SchoolClass[]>{

 return this.http.get<SchoolClass[]>(
  this.apiUrl
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