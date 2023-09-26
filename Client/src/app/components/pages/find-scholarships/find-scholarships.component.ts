import { Component } from '@angular/core';
import { ScholarshipSearchDTO } from 'src/app/models/ScholarshipSearchDTO';
import { ScholarshipUpdateDTO } from 'src/app/models/ScholarshipUpdateDTO';
import { ScholarshipService } from 'src/app/providers/scholarship.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-find-scholarships',
  templateUrl: './find-scholarships.component.html',
  styleUrls: ['./find-scholarships.component.scss'],
})
export class FindScholarshipsComponent {
  admissionName = environment.admissionName;
  emptyText = `No active ${this.admissionName} found.`;
  scholarships: ScholarshipUpdateDTO[] = [];
  scholarshipSearchDTO: ScholarshipSearchDTO = {};

  constructor(private scholarshipService: ScholarshipService) {}

  ngOnInit(): void {
    this.getScholarships();
  }

  getScholarships() {
    this.scholarshipService
      .getActiveScholarships(this.scholarshipSearchDTO)
      .then((response) => {
        this.scholarships = response;
      })
      .catch((response) => {
        console.error(response);
      });
  }
}
