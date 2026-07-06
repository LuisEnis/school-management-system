export enum UserRole {
  Director = 'Director',
  Teacher = 'Teacher',
  Student = 'Student',
  Secretary = 'Secretary'
}

export interface User {
  id: number;
  username: string;

  // NOTE: used only for login mock now
  // later backend will NEVER return password
  password: string;

  firstName: string;
  lastName: string;

  email: string;
  role: UserRole;
}