import { Component } from '@angular/core';
import { environment } from 'src/environments/environment';
import { MyApplicationShortViewDTO } from '../../../models/MyApplicationShortViewDTO';
import { ScholarshipApplicationService } from 'src/app/providers/ScholarshipApplicationService';
import { ScholarshipApplicationSearchDTO } from 'src/app/models/ScholarshipApplicationSearchDTO';
import { ScholarshipApplicationUpdateDTO } from 'src/app/models/ScholarshipApplicationUpdateDTO';
import { ScholarshipUpdateDTO } from 'src/app/models/ScholarshipUpdateDTO';

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

  async handleSearchEvent(scholarships: ScholarshipUpdateDTO[]) {
    // this.scholarships = scholarships;
  }

  getScholarships() {
    this.scholarshipApplicationService
      .getMyScholarshipApplications(this.scholarshipApplicationSearchDTO)
      .then((response) => {
        console.log(response);
        this.scholarshipApplications = response;
      })
      .catch((response) => {
        console.error(response);
      });
  }
}
