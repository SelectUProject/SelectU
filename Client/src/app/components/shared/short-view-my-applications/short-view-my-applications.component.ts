import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { MyApplicationShortViewDTO } from '../../../models/MyApplicationShortViewDTO';
import { ScholarshipApplicationUpdateDTO } from 'src/app/models/ScholarshipApplicationUpdateDTO';
import { ApplicationStatusEnum } from 'src/app/models/ApplicationStatusEnum';
import ViewApplicationDetailModalComponent from '../view-application-detail-modal/view-application-detail-modal.component';
import { MdbModalRef, MdbModalService } from 'mdb-angular-ui-kit/modal';
import { ScholarshipService } from 'src/app/providers/scholarship.service';
import { TokenService } from 'src/app/providers/token.service';
import ViewDetailsModalComponent from '../view-details-modal/view-details-modal.component';

@Component({
  selector: 'app-short-view-my-applications',
  templateUrl: './short-view-my-applications.component.html',
  styleUrls: ['./short-view-my-applications.component.scss'],
})
export class ShortViewMyApplicationsComponent {
  @Input() scholarshipApplication: ScholarshipApplicationUpdateDTO;
  @Input() statusText: string;
  @Input() statusIcon: string;

  // getStatusClass(): string[] {
  //   // Define your logic here to determine the CSS class based on statusText
  //   const classes: string[] = [];
  //   if (this.application.status === 'accepted') {
  //     classes.push('border rounded p-3 text-center fa-solid fa-check-square');
  //   } else if (this.application.status === 'denied') {
  //     classes.push('denied');
  //   } else if (this.application.status === 'uploaded') {
  //     classes.push('border rounded p-3 text-center fa-solid fa-cloud-upload');
  //   } else {
  //     classes.push('border rounded p-3 text-center fa-solid fa-spinner');
  //   }
  //   return classes;
  // }

  // getStatusIcon(): string[] {
  //   const classes: string[] = [];
  //   if (this.application.status === 'accepted') {
  //     classes.push('border rounded p-3 text-center fa-solid fa-check-square');
  //   } else if (this.application.status === 'denied') {
  //     classes.push('denied');
  //   } else if (this.application.status === 'uploaded') {
  //     classes.push('border rounded p-3 text-center fa-solid fa-cloud-upload');
  //   } else {
  //     classes.push('border rounded p-3 text-center fa-solid fa-spinner');
  //   }

  //   return classes;
  // }
}
