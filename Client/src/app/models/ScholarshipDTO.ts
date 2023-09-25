import { StatusEnum } from './StatusEnum';

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
  status: StatusEnum;
  startDate: Date;
  endDate: Date;
}
