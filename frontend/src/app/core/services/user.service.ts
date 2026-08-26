import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

import { UserDto } from '../models/users/user.dto';
import { UserDetails } from '../models/users/user-details.dto';
import { CreateUserDto } from '../models/users/create-user.dto';
import { UpdateUserDto } from '../models/users/update-user.dto';
import { ChangePasswordDto } from '../models/users/change-password.dto';
import { PagedResult } from '../models/common/paged-result.model';

@Injectable({
  providedIn:'root'
})
export class UserService {


private apiUrl =
`${environment.apiUrl}/users`;


constructor(
 private http:HttpClient
){}


getStudents(
  pageNumber: number = 1,
  pageSize: number = 15,
  search: string = '',
  sortBy: string = '',
  sortDescending: boolean = false
): Observable<PagedResult<UserDto>> {

  return this.http.get<PagedResult<UserDto>>(
    `${this.apiUrl}/students` +
    `?pageNumber=${pageNumber}` +
    `&pageSize=${pageSize}` +
    `&search=${encodeURIComponent(search)}` +
    `&sortBy=${encodeURIComponent(sortBy)}` +
    `&sortDescending=${sortDescending}`
  );

}

getAllStudents():Observable<UserDto[]>{

 return this.http.get<UserDto[]>(
 `${this.apiUrl}/students/all`
 );

}


getTeachers(
  pageNumber: number = 1,
  pageSize: number = 15,
  search: string = '',
  sortBy: string = '',
  sortDescending: boolean = false
): Observable<PagedResult<UserDto>> {

  return this.http.get<PagedResult<UserDto>>(
    `${this.apiUrl}/teachers` +
    `?pageNumber=${pageNumber}` +
    `&pageSize=${pageSize}` +
    `&search=${encodeURIComponent(search)}` +
    `&sortBy=${encodeURIComponent(sortBy)}` +
    `&sortDescending=${sortDescending}`
  );

}

getAllTeachers():Observable<UserDto[]>{

 return this.http.get<UserDto[]>(
 `${this.apiUrl}/teachers/all`
 );

}


getSecretaries(
  pageNumber: number = 1,
  pageSize: number = 15,
  search: string = '',
  sortBy: string = '',
  sortDescending: boolean = false
): Observable<PagedResult<UserDto>> {

  return this.http.get<PagedResult<UserDto>>(
    `${this.apiUrl}/secretaries` +
    `?pageNumber=${pageNumber}` +
    `&pageSize=${pageSize}` +
    `&search=${encodeURIComponent(search)}` +
    `&sortBy=${encodeURIComponent(sortBy)}` +
    `&sortDescending=${sortDescending}`
  );

}


getUserById(
 id:number
):Observable<UserDetails>{

 return this.http.get<UserDetails>(
 `${this.apiUrl}/${id}`
 );

}


create(
 dto:CreateUserDto
):Observable<UserDto>{

 return this.http.post<UserDto>(
 this.apiUrl,
 dto
 );

}


update(
 id:number,
 dto:UpdateUserDto
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


changePassword(
  dto: ChangePasswordDto
): Observable<void> {

  return this.http.put<void>(
    `${this.apiUrl}/change-password`,
    dto
  );

}

}