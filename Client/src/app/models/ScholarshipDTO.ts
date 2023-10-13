import { ScholarshipStatusEnum } from './ScholarshipStatusEnum';

export interface ScholarshipDTO {
  id: string;
  school: string;
  imageURL: string;
  value: string;
  shortDescription: string;
  description: string;
  scholarshipFormTemplate: string;
  state: string;
  city: string;
  status: ScholarshipStatusEnum;
  startDate: Date;
  endDate: Date;
}
