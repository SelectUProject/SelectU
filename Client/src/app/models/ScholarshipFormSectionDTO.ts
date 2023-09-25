import { ScholarshipFormTypeEnum } from './ScholarshipFormTypeEnum';

export interface ScholarshipFormSectionDTO {
  type: ScholarshipFormTypeEnum;
  name: string;
  required: boolean;
}
