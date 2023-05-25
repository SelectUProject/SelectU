import { GenderEnum } from './GenderEnum';

export interface UserUpdateDTO {
  id: string;
  fullName: string;
  firstName: string;
  dateOfBirth: Date;
  gender: GenderEnum;
  email: string;
  mobile: string;
  address: string;
  suburb: string;
  postcode: Number;
  state: string;
}
