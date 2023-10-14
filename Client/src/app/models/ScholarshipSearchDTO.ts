import { ScholarshipStatusEnum } from './ScholarshipStatusEnum';

export interface ScholarshipSearchDTO {
  id?: string;
  school?: string;
  description?: string;
  city?: string;
  status?: ScholarshipStatusEnum;
  value?: string;
  startDate?: Date;
  endDate?: Date;
}
