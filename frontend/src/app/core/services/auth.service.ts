import { Injectable } from '@angular/core';
import { User, UserRole } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private currentUser: User | null = null;

  private users: User[] = [
    {
      id: 1,
      username: 'admin',
      password: 'admin123',
      fullName: 'System Admin',
      role: UserRole.Director
    },
    {
      id: 2,
      username: 'teacher',
      password: 'teacher123',
      fullName: 'John Teacher',
      role: UserRole.Teacher
    },
    {
      id: 3,
      username: 'student',
      password: 'student123',
      fullName: 'Jane Student',
      role: UserRole.Student
    }
  ];

  login(username: string, password: string): User | null {
    const user = this.users.find(
      u => u.username === username && u.password === password
    );

    if (user) {
      this.currentUser = user;
      localStorage.setItem('user', JSON.stringify(user));
      return user;
    }

    return null;
  }

  logout(): void {
    this.currentUser = null;
    localStorage.removeItem('user');
  }

  getCurrentUser(): User | null {
    if (this.currentUser) return this.currentUser;

    const stored = localStorage.getItem('user');
    if (stored) {
      this.currentUser = JSON.parse(stored);
      return this.currentUser;
    }

    return null;
  }

  isLoggedIn(): boolean {
    return this.getCurrentUser() !== null;
  }
}