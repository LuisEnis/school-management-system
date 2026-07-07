import { Injectable } from '@angular/core';
import { User, UserRole } from '../models/user.model';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {


  private currentUser: User | null = null;


  constructor(
    private userService: UserService
  ){}



  login(
    username:string,
    password:string
  ): User | null {


    const user =
      this.userService
        .getUsers()
        .find(
          u =>
          u.username === username &&
          u.password === password
        );


    if(user){

      this.currentUser = user;

      localStorage.setItem(
        'user',
        JSON.stringify(user)
      );

      return user;

    }


    return null;
  }



  logout():void {

    this.currentUser = null;

    localStorage.removeItem('user');

  }



  getCurrentUser():User|null {


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