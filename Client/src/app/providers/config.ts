import { environment } from 'src/environments/environment';

export class Config {
  static api: string = environment.apiBaseUri;
  static production: boolean = environment.production;
  static dateTimezone: string = 'Australia/Melbourne';
  static dateFormat: string = ' h:mma z dddd Do MMMM YYYY';
}
