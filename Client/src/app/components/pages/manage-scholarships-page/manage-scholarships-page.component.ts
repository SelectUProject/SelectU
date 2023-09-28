import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ScholarshipShortViewDTO } from '../../../models/ScholarshipShortViewDTO';
import { ScholarshipService } from 'src/app/providers/scholarship.service';
import { ScholarshipUpdateDTO } from 'src/app/models/ScholarshipUpdateDTO';
import { ScholarshipSearchDTO } from 'src/app/models/ScholarshipSearchDTO';

@Component({
  selector: 'app-manage-scholarships-page',
  templateUrl: './manage-scholarships-page.component.html',
  styleUrls: ['./manage-scholarships-page.component.scss'],
})
export class ManageScholarshipsPageComponent implements OnInit {
  admissionName = environment.admissionName;
  emptyText = `You have no ${this.admissionName} to manage.`;
  scholarships: ScholarshipUpdateDTO[] = [];
  scholarshipSearchDTO: ScholarshipSearchDTO = {};

  constructor(private scholarshipService: ScholarshipService) {}

  ngOnInit(): void {
    this.getScholarships();
  }

  getScholarships() {
    this.scholarshipService
      .getCreatedScholarship(this.scholarshipSearchDTO)
      .then((response) => {
        this.scholarships = response;
      })
      .catch((response) => {
        console.error(response);
      });
  }
}
