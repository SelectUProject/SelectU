import { ApplicationStatusEnum } from './ApplicationStatusEnum';
import { ScholarshipFormSectionAnswerDTO } from './ScholarshipFormSectionAnswerDTO';
import { ScholarshipUpdateDTO } from './ScholarshipUpdateDTO';

export interface ScholarshipApplicationUpdateDTO {
  id: any;
  scholarshipId: any;
  scholarshipFormAnswer: ScholarshipFormSectionAnswerDTO[];
  scholarshipApplicantId: any;
  status: ApplicationStatusEnum;
  dateCreated: Date;
  scholarship: ScholarshipUpdateDTO;
}
