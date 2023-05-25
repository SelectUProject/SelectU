import { GenderEnum } from './GenderEnum';

export interface UserRegisterDTO {
  fullName: string;
  dateOfBirth: Date;
  gender: GenderEnum;
  email: string;
  password: string;
  mobile: string;
  address: string;
  suburb: string;
  postcode: Number;
  state: string;
}
