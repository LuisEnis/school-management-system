import { UserDto } from "../users/user.dto";

export interface LoginResponse {
  token: string;

  expiration: string;

  user: UserDto;
}