import { Component } from '@angular/core';
import { environment } from 'src/environments/environment';
import { MyApplicationShortViewDTO } from '../../../models/MyApplicationShortViewDTO';
import { ScholarshipApplicationService } from 'src/app/providers/application.service';
import { ScholarshipApplicationSearchDTO } from 'src/app/models/ScholarshipApplicationSearchDTO';
import { ScholarshipApplicationUpdateDTO } from 'src/app/models/ScholarshipApplicationUpdateDTO';
import { ScholarshipUpdateDTO } from 'src/app/models/ScholarshipUpdateDTO';
import { ApplicationStatusEnum } from 'src/app/models/ApplicationStatusEnum';

@Component({
  selector: 'app-my-applications',
  templateUrl: './my-applications.component.html',
  styleUrls: ['./my-applications.component.scss'],
})
export class MyApplicationsComponent {
  admissionName = environment.admissionName;
  emptyText = `You have no ${this.admissionName} applications.`;
  scholarshipApplicationSearchDTO: ScholarshipApplicationSearchDTO = {};
  scholarshipApplications: ScholarshipApplicationUpdateDTO[];

  constructor(
    private scholarshipApplicationService: ScholarshipApplicationService
  ) {}

  ngOnInit(): void {
    this.getScholarships();
  }

  async handleSearchEvent(scholarshipApplicants: ScholarshipApplicationUpdateDTO[]) {
    this.scholarshipApplications = scholarshipApplicants;
  }

  getScholarships() {
    this.scholarshipApplicationService
      .getMyScholarshipApplications(this.scholarshipApplicationSearchDTO)
      .then((response) => {
        const statusOrder = [
          ApplicationStatusEnum.Accepted,
          ApplicationStatusEnum.Submitted,
          ApplicationStatusEnum.Rejected,
        ];

        // Sort the scholarshipApplications first by status and then by createdDate
        this.scholarshipApplications = response.sort((a, b) => {
          const statusComparison =
            statusOrder.indexOf(a.status) - statusOrder.indexOf(b.status);

          if (statusComparison === 0) {
            const dateA = new Date(a.dateCreated).getTime();
            const dateB = new Date(b.dateCreated).getTime();
            return dateB - dateA; // Sort by createdDate in descending order
          }

          return statusComparison;
        });
        // this.scholarshipApplications = response;
      })
      .catch((response) => {
        console.error(response);
      });
  }
}
