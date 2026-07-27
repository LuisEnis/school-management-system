import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

import { TeacherAssignmentDto } from '../models/teachers/teacher-assignment.dto';
import { UserDto } from '../models/users/user.dto';


@Injectable({
 providedIn:'root'
})
export class TeacherService {


private apiUrl =
`${environment.apiUrl}/teacher`;


constructor(
 private http:HttpClient
){}



getClasses():Observable<TeacherAssignmentDto[]>{

 return this.http.get<TeacherAssignmentDto[]>(
 `${this.apiUrl}/classes`
 );

}



getStudentsByClass(
 classId:number
):Observable<UserDto[]>{

 return this.http.get<UserDto[]>(
 `${this.apiUrl}/classes/${classId}/students`
 );

}


}