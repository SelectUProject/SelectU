import { Role } from './Role';

export interface AuthToken {
  role: Role;
  token: string;
  expiresIn: number;
}
