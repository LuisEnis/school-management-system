import { UserDetails } from "./user-details.model";

export interface LoginResponse {
  token: string;

  expiration: string;

  user: UserDetails;
}