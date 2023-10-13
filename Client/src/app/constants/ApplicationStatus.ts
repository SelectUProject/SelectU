import { ApplicationStatusEnum } from '../models/ApplicationStatusEnum';
import { NumberLookupDTO } from '../models/LookupDTOs';

export const APPLICATION_STATUS_LIST: NumberLookupDTO[] = [
  { name: 'Submitted', value: ApplicationStatusEnum.Submitted },
  { name: 'Accepted', value: ApplicationStatusEnum.Accepted },
  { name: 'Rejected', value: ApplicationStatusEnum.Rejected },
];
