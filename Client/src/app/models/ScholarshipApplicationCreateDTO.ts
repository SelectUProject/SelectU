import { ScholarshipFormSectionAnswerDTO } from './ScholarshipFormSectionAnswerDTO';

export interface ScholarshipApplicationCreateDTO {
  scholarshipId: string;
  scholarshipFormAnswer: ScholarshipFormSectionAnswerDTO[];
}
