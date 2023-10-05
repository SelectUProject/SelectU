import { ScholarshipDTO } from './ScholarshipDTO';
import { ScholarshipFormSectionAnswerDTO } from './ScholarshipFormSectionAnswerDTO';
import { ScholarshipUpdateDTO } from './ScholarshipUpdateDTO';
import { StatusEnum } from './StatusEnum';

export interface ScholarshipApplicationUpdateDTO {
  id: any;
  scholarshipId: any;
  scholarshipFormAnswer: ScholarshipFormSectionAnswerDTO[];
  scholarshipApplicantId: any;
  status: StatusEnum;
  dateCreated: Date;
  scholarship: ScholarshipUpdateDTO;
}
