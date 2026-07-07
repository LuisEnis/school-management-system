import { Injectable } from "@angular/core";
import { User, UserRole } from "../models/user.model";

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private users: User[] = [

    {
      id: 1,
      username: 'john',
      password: '123',
      firstName: 'John',
      lastName: 'Doe',
      email: 'john@test.com',
      role: UserRole.Student
    },

    {
      id: 2,
      username: 'anna',
      password: '123',
      firstName: 'Anna',
      lastName: 'Smith',
      email: 'anna@test.com',
      role: UserRole.Student
    },

    {
      id: 3,
      username: 'luis',
      password: '123',
      firstName: 'Luis',
      lastName: 'Teacher',
      email: 'luis@teacher.com',
      role: UserRole.Teacher
    },

    {
      id: 4,
      username: 'brandon',
      password: '123',
      firstName: 'Brandon',
      lastName: 'Teacher',
      email: 'brandon@teacher.com',
      role: UserRole.Teacher
    }

  ];


  getUsers(): User[] {
    return this.users;
  }


  getUserById(id:number):User|undefined {

    return this.users.find(
      u => u.id === id
    );

  }


  getUsersByRole(role: UserRole): User[] {

    return this.users.filter(
      u => u.role === role
    );

  }


  addUser(user:User):User {

    user.id = this.users.length
      ? Math.max(...this.users.map(u => u.id)) + 1
      : 1;

    this.users.push(user);

    return user;

  }


  updateUser(
    id:number,
    updated:Partial<User>
  ):void {

    const index =
      this.users.findIndex(
        u => u.id === id
      );


    if(index !== -1){

      this.users[index] = {
        ...this.users[index],
        ...updated
      };

    }

  }


  deleteUser(id:number):void {

    this.users =
      this.users.filter(
        u => u.id !== id
      );

  }

}