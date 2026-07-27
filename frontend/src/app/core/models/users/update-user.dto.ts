import { UserRole } from "../user.model";

export interface UpdateUserDto {

    username: string;

    firstName: string;

    lastName: string;

    email: string;

    role: UserRole;
}