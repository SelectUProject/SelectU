import { Component } from '@angular/core';
import { UserUpdateDTO } from 'src/app/models/UserUpdateDTO';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-admin-page',
  templateUrl: './admin-page.component.html',
  styleUrls: ['./admin-page.component.scss']
})
export class AdminPageComponent {

admissionName= environment.admissionName;
users: UserUpdateDTO[] = [
  {
    id: '12345',
    fullName: 'will ando',
    firstName: 'will',
    dateOfBirth: new Date,
    gender: 0,
    email: 'will@selectu.com',
    mobile: '0482367383',
    address: 'my house',
    suburb: 'hawthorn',
    postcode: 3000,
    state: 'victoria',
  },
  {
    id: '12345',
    fullName: 'will ando',
    firstName: 'will',
    dateOfBirth: new Date,
    gender: 0,
    email: 'will@selectu.com',
    mobile: '0482367383',
    address: 'my house',
    suburb: 'hawthorn',
    postcode: 3000,
    state: 'victoria',
  }

]

}
