import { GenderEnum } from './GenderEnum';

export interface UserUpdateDTO {
  id: string;
  firstName: string;
  lastName: string;
  dateOfBirth: Date;
  gender: GenderEnum;
  email: string;
  phoneNumber: string;
  address: string;
  suburb: string;
  postcode: string;
  state: string;
  country: string;
}
