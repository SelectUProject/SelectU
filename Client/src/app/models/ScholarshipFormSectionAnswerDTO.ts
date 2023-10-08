import { ScholarshipFormTypeEnum } from "./ScholarshipFormTypeEnum";

export interface ScholarshipFormSectionAnswerDTO {
  name: string;
  file? : any;
  type: ScholarshipFormTypeEnum;
  value: string;
}
