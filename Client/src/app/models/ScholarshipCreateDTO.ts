import { ScholarshipFormSectionDTO } from './ScholarshipFormSectionDTO';

export interface ScholarshipCreateDTO {
  school: string;
  imageURL: string;
  value: string;
  shortDescription: string;
  description: string;
  scholarshipFormTemplate: ScholarshipFormSectionDTO[];
  city: string;
  state: string;
  startDate: Date;
  endDate: Date;
}
