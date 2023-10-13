import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ApplicationStatusEnum } from 'src/app/models/ApplicationStatusEnum';
import { ScholarshipApplicationSearchDTO } from 'src/app/models/ScholarshipApplicationSearchDTO';
import { ScholarshipApplicationUpdateDTO } from 'src/app/models/ScholarshipApplicationUpdateDTO';
import { ScholarshipUpdateDTO } from 'src/app/models/ScholarshipUpdateDTO';
import { ScholarshipApplicationService } from 'src/app/providers/application.service';
import { ScholarshipService } from 'src/app/providers/scholarship.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-view-applications-page',
  templateUrl: './view-applications-page.component.html',
  styleUrls: ['./view-applications-page.component.scss'],
})
export class ViewApplicationsPageComponent {
  admissionName = environment.admissionName;
  emptyText = `You have no ${this.admissionName} applications.`;
  scholarshipApplicationSearchDTO: ScholarshipApplicationSearchDTO = {};
  scholarshipApplications: ScholarshipApplicationUpdateDTO[] = [];
  scholarship: ScholarshipUpdateDTO;
  scholarshipId: string;

  constructor(
    private scholarshipService: ScholarshipService,
    private scholarshipApplicationService: ScholarshipApplicationService,
    private route: ActivatedRoute
  ) {
    this.scholarshipId = this.route.snapshot.url[1].path;
  }

  ngOnInit(): void {
    this.getScholarship();
    this.getApplications();
  }

  async handleSearchEvent(scholarships: ScholarshipApplicationUpdateDTO[]) {
    // this.scholarships = scholarships;
  }

  getScholarship() {
    this.scholarshipService
      .getScholarshipDetails(this.scholarshipId)
      .then((response) => {
        this.scholarship = response;
      })
      .catch((response) => {
        console.error(response);
      });
  }

  getApplications() {
    this.scholarshipApplicationService
      .getScholarshipApplications(
        this.scholarshipId,
        this.scholarshipApplicationSearchDTO
      )
      .then((response) => {
        this.scholarshipApplications = response;
      })
      .catch((response) => {
        console.error(response);
      });
  }
}
