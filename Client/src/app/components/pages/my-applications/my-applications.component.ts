import { Component } from '@angular/core';
import { environment } from 'src/environments/environment';
import { MyApplicationShortViewDTO } from '../../../models/MyApplicationShortViewDTO';

@Component({
  selector: 'app-my-applications',
  templateUrl: './my-applications.component.html',
  styleUrls: ['./my-applications.component.scss'],
})
export class MyApplicationsComponent {
  admissionName = environment.admissionName;
  emptyText = `You have no ${this.admissionName} applications.`;
  scholarships: MyApplicationShortViewDTO[] = [
    {
      image: '../../../../assets/images/testImage.png',
      applicationValue: '$1000 Dollars',
      applicationTitle: 'Tech Scholarship',
      address: 'Melbourne',
      school: 'Xavier',
      description: 'test bruh bruh bruh bruh bruh',
      status: 'pending',
    },
    {
      image: '../../../../assets/images/testImage.png',
      applicationValue: 'Life of schooling',
      applicationTitle: 'School help',
      address: 'Melbourne',
      school: 'Xavier',
      description: 'test bruh bruh bruh bruh bruh',
      status: 'uploaded',
    },
    {
      image: '../../../../assets/images/testImage.png',
      applicationValue: 'House prices',
      applicationTitle: 'Tech Money',
      address: 'Melbourne',
      school: 'Xavier',
      description: 'test bruh bruh bruh bruh bruh',
      status: 'accepted',
    },
  ];
}
