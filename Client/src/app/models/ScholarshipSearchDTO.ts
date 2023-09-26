import { StatusEnum } from './StatusEnum';

export interface ScholarshipSearchDTO {
  id?: string;
  school?: string;
  description?: string;
  city?: string;
  status?: StatusEnum;
  value?: string;
  startDate?: Date;
  endDate?: Date;
}
