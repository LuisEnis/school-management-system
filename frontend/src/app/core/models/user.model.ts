export enum UserRole {
  Director = 'Director',
  Teacher = 'Teacher',
  Student = 'Student',
  Secretary = 'Secretary'
}

export interface User {
  id: number;
  username: string;
  password: string;
  fullName: string;
  role: UserRole;
}