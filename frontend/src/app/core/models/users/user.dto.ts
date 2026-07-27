import { UserRole } from "../user.model";

export interface UserDto {

    id: number;

    firstName: string;

    lastName: string;

    fullName: string;

    email: string;

    role: UserRole;
}