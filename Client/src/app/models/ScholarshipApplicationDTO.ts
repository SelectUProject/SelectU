import { ApplicationStatusEnum } from './ApplicationStatusEnum';
import { ScholarshipDTO } from './ScholarshipDTO';

export interface ScholarshipApplicationDTO {
  id: string;
  scholarshipApplicantId: string;
  scholarship: ScholarshipDTO;
  scholarshipFormAnswer: string;
  status: ApplicationStatusEnum;
  dateCreated: Date;
}
