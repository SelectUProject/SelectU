import { ScholarshipFormTypeEnum } from './ScholarshipFormTypeEnum';

export interface ScholarshipFormSectionDTO {
  uuid: string;
  type: ScholarshipFormTypeEnum;
  name: string;
  required: boolean;
  options?: string[];
}
