import { ApplicationStatusEnum } from './ApplicationStatusEnum';

export interface ScholarshipApplicationSearchDTO {
  id?: any;
  school?: string;
  status?: ApplicationStatusEnum;
  dateCreated?: Date;
}
