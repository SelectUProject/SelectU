import { ScholarshipFormSectionDTO } from './ScholarshipFormSectionDTO';
import { ScholarshipStatusEnum } from './ScholarshipStatusEnum';

export interface ScholarshipUpdateDTO {
  id: string;
  school: string;
  imageURL: string;
  value: string;
  shortDescription: string;
  description: string;
  scholarshipFormTemplate: ScholarshipFormSectionDTO[];
  state: string;
  city: string;
  status: ScholarshipStatusEnum;
  startDate: Date;
  endDate: Date;
}
