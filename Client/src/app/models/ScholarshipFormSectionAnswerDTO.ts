import { ScholarshipFormTypeEnum } from './ScholarshipFormTypeEnum';

export interface ScholarshipFormSectionAnswerDTO {
  name: string;
  type: ScholarshipFormTypeEnum;
  value: string;
}
