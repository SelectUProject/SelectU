import { StatusEnum } from './StatusEnum';

export interface ScholarshipApplicationSearchDTO {
  id?: any;
  school?: string;
  status?: StatusEnum;
  dateCreated?: Date;
}
