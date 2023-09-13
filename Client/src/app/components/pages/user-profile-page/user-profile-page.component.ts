import { Component } from '@angular/core';
import { UserUpdateDTO } from 'src/app/models/UserUpdateDTO';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-user-profile-page',
  templateUrl: './user-profile-page.component.html',
  styleUrls: ['./user-profile-page.component.scss']
})
export class UserProfilePageComponent {

  admissionName = environment.admissionName;
  
  user: UserUpdateDTO = {
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


}
