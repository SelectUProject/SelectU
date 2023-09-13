import { Component } from '@angular/core';
import { UserUpdateDTO } from 'src/app/models/UserUpdateDTO';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-admin-page',
  templateUrl: './admin-page.component.html',
  styleUrls: ['./admin-page.component.scss'],
})
export class AdminPageComponent {
  admissionName = environment.admissionName;
  // users: UserUpdateDTO[] = [
  //   {
  //     Id: '12345',
  //     FirstName: 'will',
  //     LastName: 'ando',
  //     DateOfBirth: new Date,
  //     Gender: 0,
  //     Email: 'will@selectu.com',
  //     PhoneNumber: '0482367383',
  //     Address: 'my house',
  //     Suburb: 'hawthorn',
  //     Postcode: '3000',
  //     State: 'victoria',
  //     Country: 'straya',
  //   },
  //   {
  //     Id: '12345',
  //     FirstName: 'will',
  //     LastName: 'ando',
  //     DateOfBirth: new Date,
  //     Gender: 0,
  //     Email: 'will@selectu.com',
  //     PhoneNumber: '0482367383',
  //     Address: 'my house',
  //     Suburb: 'hawthorn',
  //     Postcode: '3000',
  //     State: 'victoria',
  //     Country: 'straya',
  //   }
  // ]
}
