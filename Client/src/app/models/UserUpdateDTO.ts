import { GenderEnum } from './GenderEnum';

export interface UserUpdateDTO {
  Id: string;
  FirstName: string;
  LastName: string;
  DateOfBirth: Date;
  Gender: GenderEnum;
  Email: string;
  PhoneNumber: string;
  Address: string;
  Suburb: string;
  Postcode: string;
  State: string;
  Country: string;
}
