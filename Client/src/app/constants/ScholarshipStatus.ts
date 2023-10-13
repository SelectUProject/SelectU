import { NumberLookupDTO } from '../models/LookupDTOs';
import { ScholarshipStatusEnum } from '../models/ScholarshipStatusEnum';

export const SCHOLARSHIP_STATUS_LIST: NumberLookupDTO[] = [
  { name: 'Draft', value: ScholarshipStatusEnum.Draft },
  { name: 'Active', value: ScholarshipStatusEnum.Active },
  { name: 'Archived', value: ScholarshipStatusEnum.Archived },
];
