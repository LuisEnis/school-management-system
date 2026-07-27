import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { LoginRequest } from '../models/auth/login-request.dto';
import { LoginResponse } from '../models/auth/login-response.dto';
import { UserDetails } from '../models/users/user-details.dto';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {


  private apiUrl = `${environment.apiUrl}/auth`;
  
  private currentUser: UserDetails | null = null;


  constructor(
      private http: HttpClient
  ){}



  login(
      request: LoginRequest
  ): Observable<LoginResponse> {

      return this.http
          .post<LoginResponse>(
              `${this.apiUrl}/login`,
              request
          )
          .pipe(
              tap(response => {

                  localStorage.setItem(
                      'token',
                      response.token
                  );
              })
          );
  }


  logout(): void {

      this.currentUser = null;

      localStorage.removeItem('token');
      localStorage.removeItem('user');

  }

  getToken(): string | null {

      return localStorage.getItem('token');

  }

  getMe(): Observable<UserDetails> {

    return this.http.get<UserDetails>(
        `${this.apiUrl}/me`
    );

    }

  loadCurrentUser(): Observable<UserDetails> {

    return this.getMe()
      .pipe(
        tap(user => {

          this.currentUser = user;

          localStorage.setItem(
            'user',
            JSON.stringify(user)
          );

        })
      );

  }

  getCurrentUser(): UserDetails | null {

      if(this.currentUser)
          return this.currentUser;


      const stored =
          localStorage.getItem('user');


      if(stored){

          this.currentUser =
              JSON.parse(stored);

          return this.currentUser;

      }


      return null;
  }



  isLoggedIn():boolean {

    return this.getCurrentUser() !== null;

  }

}