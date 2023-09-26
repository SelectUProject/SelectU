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
  emptyText = `You have no ${this.admissionName} to manage.`;
  scholarships: ScholarshipShortViewDTO[] = [
    // {
    //   image: '../../../../assets/images/testImage.png',
    //   scholarshipValue: '$1000 Dollars',
    //   scholarshipTitle: 'Tech Scholarship',
    //   address: 'Melbourne',
    //   school: 'Xavier',
    //   description: 'test asdasd asd as das das das dman',
    // },
    // {
    //   image: '../../../../assets/images/testImage.png',
    //   scholarshipValue: 'Life of schooling',
    //   scholarshipTitle: 'School help',
    //   address: 'Melbourne',
    //   school: 'Xavier',
    //   description: 'testas dasd as das das das das man',
    // },
    // {
    //   image: '../../../../assets/images/testImage.png',
    //   scholarshipValue: 'House prices',
    //   scholarshipTitle: 'Tech Money',
    //   address: 'Melbourne',
    //   school: 'Xavier',
    //   description: 'testd as das das das das das das das as  man',
    // },
  ];
}
