import { ScholarshipDTO } from './ScholarshipDTO';
import { StatusEnum } from './StatusEnum';

export interface ScholarshipApplicationDTO {
  id: string;
  scholarshipApplicantId: string;
  scholarship: ScholarshipDTO;
  scholarshipFormAnswer: string;
  status: StatusEnum;
  dateCreated: Date;
}
