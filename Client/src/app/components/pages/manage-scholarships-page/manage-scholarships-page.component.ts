import { Component } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ScholarshipShortViewDTO } from '../../../models/ScholarshipShortViewDTO';

@Component({
  selector: 'app-manage-scholarships-page',
  templateUrl: './manage-scholarships-page.component.html',
  styleUrls: ['./manage-scholarships-page.component.scss'],
})
export class ManageScholarshipsPageComponent {
  admissionName = environment.admissionName;
  scholarships: ScholarshipShortViewDTO[] = [
    {
      image: 'testImage',
      scholarshipValue: '$1000 Dollars',
      address: 'Melbourne',
      school: 'Xavier',
      description: 'test asdasd asd as das das das dman',
    },
    {
      image: 'testImage',
      scholarshipValue: 'Life of schooling',
      address: 'Melbourne',
      school: 'Xavier',
      description: 'testas dasd as das das das das man',
    },
    {
      image: 'testImage',
      scholarshipValue: 'House prices',
      address: 'Melbourne',
      school: 'Xavier',
      description: 'testd as das das das das das das das as  man',
    },
  ];
}
