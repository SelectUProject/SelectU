import { GenderEnum } from './GenderEnum';

export interface CreateScholarshipDTO {
  school: string;
  imageURL: string;
  value: string;
  shortDescription: string;
  description: Date;
  scholarshipFormTemplate: GenderEnum;
  city: string;
  state: string;
  startDate: Date;
  endDate: Date;
}
