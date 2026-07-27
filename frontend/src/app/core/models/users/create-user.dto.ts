import { UserRole } from "../user.model";

export interface CreateUserDto {

    username: string;

    firstName: string;

    lastName: string;

    email: string;

    password: string;

    role: UserRole;
}