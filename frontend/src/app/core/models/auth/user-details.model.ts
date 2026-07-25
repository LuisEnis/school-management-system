import { UserRole } from "../user.model";

export interface UserDetails {
  id: number;

  username: string;

  firstName: string;

  lastName: string;

  email: string;

  role: UserRole;
}