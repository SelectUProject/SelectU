import { GenderEnum } from './GenderEnum';

export interface CreateScholarshipDTO {
  email: string;
  firstName: string;
  lastName: string;
  password: string;
  dateOfBirth: Date;
  gender: GenderEnum;
  phoneNumber: string;
  state: string;
  country: string;
}
