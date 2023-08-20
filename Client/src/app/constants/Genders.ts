import { GenderEnum } from '../models/GenderEnum';
import { NumberLookupDTO } from '../models/LookupDTOs';

export const GENDERS_LIST: NumberLookupDTO[] = [
  { name: 'Male', value: GenderEnum.Male },
  { name: 'Female', value: GenderEnum.Female },
  { name: 'I prefer not to say', value: GenderEnum.Other },
];
