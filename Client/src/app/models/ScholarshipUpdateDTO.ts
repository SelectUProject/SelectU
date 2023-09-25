import { ScholarshipFormSectionDTO } from './ScholarshipFormSectionDTO';
import { StatusEnum } from './StatusEnum';

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
  status: StatusEnum;
  startDate: Date;
  endDate: Date;
}
